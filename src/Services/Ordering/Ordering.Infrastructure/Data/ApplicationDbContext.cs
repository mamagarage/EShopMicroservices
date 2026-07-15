using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ordering.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Customer>(entity =>
        //{
        //    entity.HasKey(e => e.Id);
        //    entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        //    entity.Property(e => e.Email).IsRequired();
        //});

        //modelBuilder.Entity<Customer>().Property(e => e.Name).IsRequired().HasMaxLength(100);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

}
