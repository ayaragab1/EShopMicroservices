using Basket.Api.Dtos;
using BuildingBlocks.Messaging.Events;
using JasperFx.Events.Daemon;
using MassTransit;

namespace Basket.Api.Basket.CheckoutBasket;


public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto) :ICommand<CheckoutBasketResult>; 
public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketValidator: AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketValidator()
    {
        RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto can't be null");
        RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class CheckoutBasketHandler(
    IBasketRepository repository, IPublishEndpoint publishEndpoint) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        
        // get existing basket with total price
        var basket = await repository.GetBasketAsync(command.BasketCheckoutDto.UserName, cancellationToken);
        if (basket == null)
        {
            return new CheckoutBasketResult(false);
        }
        // Set total price on basket checkout event message
        var eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;

        // send basket checkout event to rabbitmq using masstransit
        await publishEndpoint.Publish(eventMessage, cancellationToken);

        // delete the basket
        await repository.DeleteBasketAsync(command.BasketCheckoutDto.UserName, cancellationToken);

        return new CheckoutBasketResult(true);
    }
}