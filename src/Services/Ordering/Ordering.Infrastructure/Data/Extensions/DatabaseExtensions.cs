using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure.Data.Extensions;

public static class DatabaseExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // What does it mean this instruction? It means that the database will be created
        // if it does not exist, and if it exists, it will apply any pending migrations to the database.
        // This is useful for development and testing scenarios
        // where you want to ensure that the database schema is up to date with the latest migrations.
        context.Database.MigrateAsync().GetAwaiter().GetResult();

        await SeedDataAsync(context);
    }

    private static async Task SeedDataAsync(ApplicationDbContext context)
    {
        await SeedCustomerAsync(context);
        await SeedProductDataAsync(context);
        await SeedOrderAndItemsAsync(context);
    }

    private static async Task SeedProductDataAsync(ApplicationDbContext context)
    {
        if (!await context.Products.AnyAsync())
        {
            await context.Products.AddRangeAsync(InitialData.Products);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedOrderAndItemsAsync(ApplicationDbContext context)
    {
        if (!await context.Orders.AnyAsync())
        {
            await context.Orders.AddRangeAsync(InitialData.OrdersWithItems);
            await context.SaveChangesAsync();
        }
    }   

    private static async Task SeedCustomerAsync(ApplicationDbContext context)
    {
        // Check if the database is empty and seed initial data if necessary
        if (!await context.Customers.AnyAsync())
        {
            await context.Customers.AddRangeAsync(InitialData.Customers);
            await context.SaveChangesAsync();
        }
    }
}
