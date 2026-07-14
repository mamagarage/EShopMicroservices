namespace Ordering.Domain.ValueObjects;

public record CustomerId
{
    public Guid Value { get; }
    
    private CustomerId(Guid value) => Value = value;

    /// <summary>
    /// Creates a new instance of <see cref="CustomerId"/> with the specified value.
    /// what king of design pattern is this? 
    /// This is a factory method pattern. It is used to create an instance of a class without 
    /// exposing the instantiation logic to the client and refers to the newly created object through a common interface.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    public static CustomerId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("CustomerId cannot be empty.");
        }

        return new CustomerId(value);
    }
}
