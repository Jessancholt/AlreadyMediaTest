using Microsoft.Extensions.DependencyInjection;
using Test.DatasetClient.Clients;
using Test.DatasetClient.Clients.Interfaces;

namespace Test.DatasetClient.Configurations;

public static class DatasetClientConfigurationExtensions
{
    public static IServiceCollection AddDatasetClient(this IServiceCollection services)
    {
        // clients
        services.AddScoped<INasaDatasetClient, NasaDatasetClient>();

        return services;
    }
}
