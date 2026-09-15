using MassTransit;
using Microsoft.FeatureManagement;

namespace Ordering.Application.Orders.EventHandlers.Domain;

public class OrderCreatedEventHandler
    (IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

        // Check if the "OrderFulfillment" feature is enabled
        if (await featureManager.IsEnabledAsync("OrderFulfillment"))
        {
            // Map the domain event to an integration event
            var orderCreatedIntegrationEvent = domainEvent.order.ToOrderDto();

            // Publish the integration event to the message bus
            await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
        }
    }
}
