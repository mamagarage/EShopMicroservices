using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public static class Extentions
{
    public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DiscountContext>();

        // Apply any pending migrations to the database
        // This will create the database if it does not exist and apply any migrations
        // This is useful for development and testing environments
        // In production, you may want to handle migrations differently
        // For example, you may want to run migrations manually or use a CI/CD pipeline to apply migrations
        // You can also use dbContext.Database.EnsureCreated() to create the database if it does not exist
        // However, this will not apply any migrations and is not recommended for production environments
        dbContext.Database.MigrateAsync();

        return app;
    }
}