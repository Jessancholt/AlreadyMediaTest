using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Test.DataAccess.Entities;
using Test.DataAccess.Repositories;
using Test.DataAccess.Storages;
using Test.DataAccess.Storages.Interfaces;

namespace Test.DataAccess.Configurations;

public static class DataAccessConfigurationExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, Action<DbContextOptionsBuilder> dbSetup)
    {
        // uow
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // finders
        services.AddScoped<IFinder<Meteorite>, Finder<Meteorite>>();
        services.AddScoped<IFinder<MeteoriteClass>, Finder<MeteoriteClass>>();
        services.AddScoped<IFinder<DiscoveryYear>, Finder<DiscoveryYear>>();

        // repositories
        services.AddScoped<IRepository<Meteorite>, Repository<Meteorite>>();
        services.AddScoped<IRepository<MeteoriteClass>, Repository<MeteoriteClass>>();
        services.AddScoped<IRepository<DiscoveryYear>, Repository<DiscoveryYear>>();

        services.AddDbContext<TestDbContext>(dbSetup);

        return services;
    }
}
