using Microsoft.Extensions.DependencyInjection;
using Test.Core.Models;
using Test.Core.Services;
using Test.Core.Services.Interfaces;
using Test.Core.Workers;

namespace Test.Core.Configurations;

public static class CoreConfigurationExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        // services
        services.AddScoped<IMeteoriteService, MeteoriteService>();
        services.AddScoped<ICacheService<string>, CacheService<string>>();

        // background
        services.AddHostedService<DatasetBackgroundService>();

        return services;
    }
}
