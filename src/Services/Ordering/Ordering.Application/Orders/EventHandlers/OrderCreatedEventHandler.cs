namespace Ordering.Application.Orders.EventHandlers;

/// <summary>
/// OrderCreatedEventHandler 
/// </summary>
/// <param name="logger"></param>
public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
    public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}
