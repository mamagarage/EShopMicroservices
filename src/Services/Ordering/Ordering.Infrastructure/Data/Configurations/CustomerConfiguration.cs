using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        // what HasConversion does is it tells EF Core how to convert
        // the CustomerId value object to and from the database representation.
        // In this case, we are converting the CustomerId to its underlying Guid value when saving to the database,
        // and converting it back to a CustomerId when reading from the database.
        
        builder.Property(c => c.Id).HasConversion(
                // The first lambda expression specifies how to convert the CustomerId to a database value (Guid).
                customerId => customerId.Value,
                // The second lambda expression specifies how to convert the database value (Guid) back to a CustomerId.
                dbId => CustomerId.Of(dbId));

        builder.Property(c => c.Name).HasMaxLength(100).IsRequired();

        builder.Property(c => c.Email).HasMaxLength(255);
        builder.HasIndex(c => c.Email).IsUnique();
    }
}
