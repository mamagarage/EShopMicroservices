
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto)
    : ICommand<CheckoutBasketResult>;

public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator
    : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto can't be null");
        RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class CheckoutBasketCommandHandler
    (IBasketRepository repository, IPublishEndpoint publishEndpoint)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        // get existing basket with total price
        var basket = await repository.GetBasket(command.BasketCheckoutDto.UserName, cancellationToken);
        
        if (basket == null)
        {
            return new CheckoutBasketResult(false);
        }

        // create basket checkout event message mapping from BasketCheckoutDto to BasketCheckoutEvent
        BasketCheckoutEvent eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
        
        // Set totalprice on basketcheckout event message
        eventMessage.TotalPrice = basket.TotalPrice;

        // send the event message to RabbitMQ using MassTransit

        //what is the cancellation token? 
        // The cancellation token is a mechanism that allows you to cancel an ongoing operation. It is typically used in asynchronous programming to signal that an operation should be canceled before it completes. In this context, the cancellation token is passed to the Publish method of the IPublishEndpoint interface, allowing the publish operation to be canceled if needed.
        await publishEndpoint.Publish(eventMessage, cancellationToken);

        // delete the basket
        await repository.DeleteBasket(command.BasketCheckoutDto.UserName, cancellationToken);

        return new CheckoutBasketResult(true);
    }
}