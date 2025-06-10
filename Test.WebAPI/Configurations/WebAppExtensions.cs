using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Test.Core.Configurations;
using Test.DataAccess.Configurations;
using Test.DatasetClient.Configurations;
using Test.Shared.Configurations;

namespace Test.WebAPI.Configurations;

internal static class WebAppExtensions
{
    public static WebApplicationBuilder ConfigureServer(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<CacheOptions>(builder.Configuration.GetSection(Config.CACHE_OPTIONS));
        builder.Services.Configure<BackgroundOptions>(builder.Configuration.GetSection(Config.BACKGROUND_OPTIONS));
        builder.Services.Configure<DatasetClientOptions>(builder.Configuration.GetSection(Config.DATASET_OPTIONS));

        builder.Services.AddDataAccess(option =>
        {
            option.UseLazyLoadingProxies();
            option.UseSqlite(builder.Configuration.GetConnectionString(Config.DEFAULT_CONNECTION_KEY));
        });

        builder.Services.AddCore();
        builder.Services.AddDatasetClient();

        builder.Services.AddSwagger();
        builder.Services.AddMemoryCache();
        builder.Services.AddHttpClient();
        builder.Services.AddControllers();
        builder.Services.AddFluentValidation();

        var serverOptions = new ServerOptions();
        builder.Configuration.GetSection(Config.SERVER_OPTIONS).Bind(serverOptions);

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(cors =>
            {
                cors.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithExposedHeaders(HeaderNames.ContentDisposition);

                cors.WithOrigins(serverOptions.AllowedOrigin);
            });
        });

        return builder;
    }
}
