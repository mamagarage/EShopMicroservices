namespace Ordering.Domain.Models;

public class OrderItem : Entity<OrderItemId>
{
    // Why internal constructor? The internal constructor is used to restrict the creation of OrderItem instances
    // to within the same assembly. This is a common practice in domain-driven design to ensure that entities
    // are created in a controlled manner, often through factory methods or aggregate roots,
    // rather than directly instantiated from outside the assembly. It helps maintain the integrity
    // of the domain model by enforcing business rules and invariants during the creation process.
    // In this case, what the agggregate root (Order) is responsible for creating and managing OrderItem instances,
    // ensuring that they are always in a valid state.
    internal OrderItem(OrderId orderId, ProductId productId, int quantity, decimal price)
    {
        this.Id = OrderItemId.Of(Guid.NewGuid());
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }


    public OrderId OrderId { get; private set; } = default!;

    public ProductId ProductId { get; private set; } = default!;

    public int Quantity { get; private set; } = default;

    public decimal Price { get; private set; } = default;
}
