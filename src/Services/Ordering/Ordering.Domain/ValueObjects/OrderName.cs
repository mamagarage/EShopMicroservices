namespace Ordering.Domain.ValueObjects;

// why are we using record here instead of class?
// because record is immutable and value-based equality, which is suitable for value objects
// what does it mean immutable? it means that once an instance of the record is created,
// its properties cannot be changed. This is important for value objects because they represent a value rather
// than an entity with an identity.
// Value objects should not change their state after creation,
// ensuring consistency and reliability in the domain model.

// why the order name cannot change? because the order name is a value object,
// and value objects should be immutable. Once an order name is created, it should not change, as it represents a specific value in the domain.
// Changing the order name would violate the principles of value objects and could lead to inconsistencies in the domain model.
public record OrderName
{
    private const int DefaultLength = 5;
    public string Value { get; }
    private OrderName(string value) => Value = value;
    //public static OrderName Of(string value)
    //{
    //    ArgumentException.ThrowIfNullOrWhiteSpace(value);
    //    ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length, DefaultLength);

    //    return new OrderName(value);
    //}
}