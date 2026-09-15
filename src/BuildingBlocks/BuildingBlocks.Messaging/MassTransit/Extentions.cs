using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit;
public static class Extentions
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null) 
    {
        // Add MassTransit and configure RabbitMQ

        // What is MassTransit? MassTransit is a free, open-source distributed application framework for .NET,
        // simplifying the development of message-based applications.
        // It provides a consistent programming model for building message-driven applications,
        // allowing developers to focus on business logic rather than the complexities of messaging infrastructure.
        services.AddMassTransit(config =>
        {
            // Set the endpoint name formatter to use kebab-case for endpoint names
            config.SetKebabCaseEndpointNameFormatter();

            // Add consumers from the specified assembly if provided
            if (assembly != null)
                config.AddConsumers(assembly);

            // Configure RabbitMQ as the message broker
            config.UsingRabbitMq((context, configurator) =>
            {
                // Configure the RabbitMQ host using settings from the configuration
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                {
                    host.Username(configuration["MessageBroker:UserName"]);
                    host.Password(configuration["MessageBroker:Password"]);
                });

                // Configure endpoints for the consumers
                configurator.ConfigureEndpoints(context);
            });
        });



        return services;
    }
}