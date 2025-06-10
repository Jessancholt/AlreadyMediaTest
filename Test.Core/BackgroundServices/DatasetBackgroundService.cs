using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Test.Core.Configurations;
using Test.Core.Converters;
using Test.Core.Models;
using Test.Core.Models.Predefined;
using Test.Core.Services.Interfaces;
using Test.DataAccess.Entities;
using Test.DataAccess.Storages.Interfaces;
using Test.DatasetClient.Clients.Interfaces;
using Test.DatasetConnect.Models;

namespace Test.Core.Workers;

internal class DatasetBackgroundService : BackgroundService
{
    private readonly BackgroundOptions _settings;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<DatasetBackgroundService> _logger;

    public DatasetBackgroundService(
        IOptions<BackgroundOptions> options,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<DatasetBackgroundService> logger)
    {
        _settings = options.Value;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            try
            {
                var meteoriteFinder = uow.Get<IFinder<Meteorite>>();
                var classFinder = uow.Get<IFinder<MeteoriteClass>>();
                var yearFinder = uow.Get<IFinder<DiscoveryYear>>();

                var meteoriteRepository = uow.Get<IRepository<Meteorite>>();
                var classRepository = uow.Get<IRepository<MeteoriteClass>>();
                var yearRepository = uow.Get<IRepository<DiscoveryYear>>();

                var datasetClient = scope.ServiceProvider.GetRequiredService<INasaDatasetClient>();
                var cache = scope.ServiceProvider.GetRequiredService<ICacheService<string>>();

                var data = await datasetClient.GetAsync(cancellationToken);
                if (data is not null)
                {
                    await uow.BeginTransactionAsync(cancellationToken);

                    var entities = meteoriteFinder.GetAsync(asNoTracking: true);
                    var classes = await classFinder.GetAsync().ToListAsync(cancellationToken);
                    var years = await yearFinder.GetAsync().ToListAsync(cancellationToken);

                    var changes = await GetChangesAsync(entities, classes, years, data.ToDictionary(x => x.Id), cancellationToken);

                    if (changes.Classes.Has)
                    {
                        await classRepository.CreateAsync(changes.Classes.Added, cancellationToken);

                        cache.Invalidate(CacheConstants.GET_CLASSES_ID);
                    }

                    if (changes.Years.Has)
                    {
                        await yearRepository.CreateAsync(changes.Years.Added, cancellationToken);

                        cache.Invalidate(CacheConstants.GET_YEARS_ID);
                    }

                    if (changes.Meteorites.Has)
                    {
                        meteoriteRepository.Update(changes.Meteorites.Updated);
                        await meteoriteRepository.CreateAsync(changes.Meteorites.Added, cancellationToken);
                        meteoriteRepository.Delete(changes.Meteorites.Removed);

                        cache.Invalidate(CacheConstants.GET_METEORITES_ID);
                    }

                    await uow.CommitAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                await uow.RollbackAsync(cancellationToken);
                _logger.LogError(ex, ex.Message);
            }

            await Task.Delay(_settings.CheckInterval, cancellationToken);
        }
    }

    private async Task<MeteoriteChanges> GetChangesAsync(IAsyncEnumerable<Meteorite> entities, List<MeteoriteClass> classes, List<DiscoveryYear> years, Dictionary<int, DatasetResponse> data, CancellationToken cancellationToken)
    {
        var changes = new MeteoriteChanges();

        var processedCodes = new HashSet<int>();

        await foreach (var entity in entities)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!data.TryGetValue(entity.UniqueId, out var item))
            {
                changes.Meteorites.Removed.Add(entity);
            }

            var meteorite = ProcessMeteoriteMetadata(item, entity.Id, classes, years, changes);
            if (meteorite == entity)
            {
                continue;
            }

            changes.Meteorites.Updated.Add(meteorite);
            processedCodes.Add(entity.UniqueId);
        }

        foreach (var (_, item) in data.Where(x => !processedCodes.Contains(x.Key)))
        {
            var meteorite = ProcessMeteoriteMetadata(item, Guid.NewGuid(), classes, years, changes);
            changes.Meteorites.Added.Add(meteorite);
        }

        return changes;
    }

    private Meteorite ProcessMeteoriteMetadata(DatasetResponse item, Guid id, List<MeteoriteClass> classes, List<DiscoveryYear> years, MeteoriteChanges changes)
    {
        if (!item.TryConvert(id, classes, years, out var meteorite))
        {
            return meteorite;
        }

        if (meteorite.Class is null)
        {
            var cl = changes.Classes.Added.FirstOrDefault(x => x.Name == item.Recclass);
            if (cl is null)
            {
                cl = new MeteoriteClass
                {
                    Name = item.Recclass,
                };
                changes.Classes.Added.Add(cl);
            }
            meteorite.Class = cl;
        }

        if (meteorite.Year is null)
        {
            var year = changes.Years.Added.FirstOrDefault(x => x.Year == item.Year.Year);
            if (year is null)
            {
                year = new DiscoveryYear()
                {
                    Year = item.Year.Year,
                };
                changes.Years.Added.Add(year);
            }
            meteorite.Year = year;
        }

        return meteorite;
    }
}
