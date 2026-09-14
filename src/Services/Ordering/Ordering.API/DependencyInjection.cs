using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {

        // Why Carter service?
        // Carter is a library that simplifies building APIs in .NET by providing a lightweight framework for defining routes, handling requests, and managing responses. It allows developers to create APIs with minimal boilerplate code, making it easier to focus on the business logic of the application. By using Carter, developers can quickly set up endpoints, handle HTTP methods,
        // and manage request/response processing in a more streamlined manner compared to traditional ASP.NET Core controllers.
        services.AddCarter();

        // Why CustomExceptionHandler service?
        // The CustomExceptionHandler service is used to handle exceptions that occur during the processing of HTTP requests in a centralized manner. 
        // It allows developers to define custom logic for handling different types of exceptions, logging error details,
        // and returning appropriate HTTP responses to clients. By using a custom exception handler,
        // developers can ensure consistent error handling across the application,
        // improve debugging and monitoring, and provide meaningful feedback to API consumers when errors occur.
        services.AddExceptionHandler<CustomExceptionHandler>();

        // Why HealthChecks service?
        // The HealthChecks service is used to monitor the health and status of an application or its dependencies.
        services.AddHealthChecks()
            .AddSqlServer(
                connectionString: configuration.GetConnectionString("Database")!,
                name: "OrderingDB-check",
                tags: new[] { "orderingdb" });

        return services;
    }


    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter();

        app.UseExceptionHandler(exceptionHandlerApp => { });

        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}