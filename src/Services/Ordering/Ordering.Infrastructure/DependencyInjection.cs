using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Data;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    // Extension method to add infrastructure services to the IServiceCollection
    public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        services.AddScoped<AuditableEntityInterceptor>();
        services.AddScoped<DispatchDomainEventsInterceptor>();


        // Add services related to the infrastructure layer, such as database context, repositories, etc.
        services.AddDbContext<ApplicationDbContext>((sp, options) => 
        { 
            options.AddInterceptors(
                sp.GetService<ISaveChangesInterceptor>()
            );
            options.UseSqlServer(connectionString); 
        });

        // Register the ApplicationDbContext as the implementation of IApplicationDbContext
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        return services;
    }
}