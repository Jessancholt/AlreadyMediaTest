using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;

namespace Test.WebAPI.Configurations;

internal static class ValidationConfigurationExtensions
{
    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}
