namespace Ordering.Domain.ValueObjects;

/// <summary>
/// Represents a payment method used in an order, including card details and payment method type.
/// why Payment is value object? Because it is immutable and represents a concept rather than an entity with identity.
/// what does it mean "Identity" in this context? In the context of domain-driven design, 
/// "identity" refers to the unique identifier that distinguishes one entity from another. 
/// Value objects, like Payment, do not have a unique identity; they are defined by their attributes and are considered equal if their attributes are the same.
/// </summary>
public record Payment
{
    public string? CardName { get; } = default!;
    public string CardNumber { get; } = default!;
    public string Expiration { get; } = default!;
    public string CVV { get; } = default!;
    public int PaymentMethod { get; } = default!;

    protected Payment()
    {
    }

    private Payment(string cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
    {
        CardName = cardName;
        CardNumber = cardNumber;
        Expiration = expiration;
        CVV = cvv;
        PaymentMethod = paymentMethod;
    }

    public static Payment Of(string cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(expiration);
        ArgumentException.ThrowIfNullOrWhiteSpace(cvv);

        ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3);

        return new Payment(cardName, cardNumber, expiration, cvv, paymentMethod);
    }

}
