using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPIServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddCarter();

        return services;
    }


    public static WebApplication UseApiServices(this WebApplication app, IConfiguration configuration)
    {
        // app.UseCarter();

        return app;
    }
}