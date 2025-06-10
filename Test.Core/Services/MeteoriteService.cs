using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Test.Core.Models;
using Test.Core.Models.Predefined;
using Test.Core.Services.Interfaces;
using Test.DataAccess.Entities;
using Test.DataAccess.Storages.Interfaces;
using Test.Shared.Exceptions;

namespace Test.Core.Services;

internal sealed class MeteoriteService : IMeteoriteService
{
    private readonly ICacheService<string> _cache;
    private readonly IFinder<Meteorite> _meteoriteFinder;
    private readonly IFinder<MeteoriteClass> _classFinder;
    private readonly IFinder<DiscoveryYear> _yearFinder;
    private readonly ILogger<MeteoriteService> _logger;

    public MeteoriteService(
        ICacheService<string> cache,
        IFinder<Meteorite> meteoriteFinder,
        IFinder<MeteoriteClass> classFinder,
        IFinder<DiscoveryYear> yearFinder,
        ILogger<MeteoriteService> logger)
    {
        _cache = cache;
        _meteoriteFinder = meteoriteFinder;
        _classFinder = classFinder;
        _yearFinder = yearFinder;
        _logger = logger;
    }

    public async Task<MeteoriteContext> GetAsync(MeteoritesFilter filter, CancellationToken cancellationToken)
    {
        try
        {
            var meteorites = await _meteoriteFinder
                .GetAsync(x => x.Name == filter.Name
                                || x.Class.Name == filter.Recclass
                                || x.Year.Year > filter.From
                                || x.Year.Year < filter.To
                                || filter.IsEmpty)
                .Select(entity => new MeteoriteData(entity))
                .ToListAsync(cancellationToken);

            var classes = await _cache.GetAsync(CacheConstants.GET_CLASSES_ID, async () => await _classFinder.GetAsync().Select(x => x.Name).OrderBy(x => x).ToListAsync(cancellationToken));
            var years = await _cache.GetAsync(CacheConstants.GET_YEARS_ID, async () => await _yearFinder.GetAsync().Select(x => x.Year).OrderBy(x => x).ToListAsync(cancellationToken));

            return new MeteoriteContext()
            {
                Data = meteorites,
                PossibleClasses = classes,
                PossibleYears = years,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw new CoreException("Failed to get meteorites by filters");
        }
    }
}
