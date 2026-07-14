namespace Ordering.Domain.Models;

internal class Customer : Entity<CustomerId>
{
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;

    // Factory method to create a new Customer instance
    // This method ensures that the Customer is created with valid data
    // and encapsulates the creation logic within the Customer class
    // Why a factory method? It provides a clear and controlled way to create instances of the Customer class,
    // ensuring that all necessary validations are performed before an instance is created.
    // This approach helps maintain the integrity of the Customer entity and prevents the creation of invalid objects.
    public static Customer Create(CustomerId customerId, string name, string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        var customer = new Customer
        {
            Id = customerId,
            Name = name,
            Email = email
        };

        return customer;
    }
}