using BuildingBlocks.Behaviors;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using System.Reflection;

namespace Ordering.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices
        (this IServiceCollection services, IConfiguration configuration)
    {

        // Add MediatR services and register handlers from the current assembly
        // Why MediatR? MediatR is a popular library for implementing the Mediator pattern in .NET applications.
        // It helps to decouple the sender and receiver of requests, promoting a clean architecture and separation of concerns.
        // By using MediatR, we can easily manage commands, queries, and events in our application, making it more maintainable and testable.
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            // Add custom behaviors delegates for MediatR pipeline
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });


        // Why Feature Management? Feature Management allows us to enable or disable features in our application
        // dynamically without redeploying the code.
        // This is particularly useful for A/B testing, gradual rollouts, and managing feature flags in production environments.
        // By using Feature Management, we can control the availability of features based on configuration settings,
        // user roles, or other criteria, providing a more flexible and controlled approach to feature deployment.
        services.AddFeatureManagement();

        // Add message broker configuration for MassTransit
        services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

        return services;
    }
}