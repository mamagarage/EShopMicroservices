using BuildingBlocks.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ordering.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            // Add custom behaviors delegates for MediatR pipeline
            config.AddOpenBehavior(typeof(BuildingBlocks.Behaviors.ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(BuildingBlocks.Behaviors.LoggingBehavior<,>));
        });

        return services;
    }
}