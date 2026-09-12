using Carter;

namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {

        // Why Carter service?
        // Carter is a library that simplifies building APIs in .NET by providing a lightweight framework for defining routes, handling requests, and managing responses. It allows developers to create APIs with minimal boilerplate code, making it easier to focus on the business logic of the application. By using Carter, developers can quickly set up endpoints, handle HTTP methods,
        // and manage request/response processing in a more streamlined manner compared to traditional ASP.NET Core controllers.
        services.AddCarter();

        return services;
    }


    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter();

        return app;
    }
}