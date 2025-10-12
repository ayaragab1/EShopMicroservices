using Discount.Grpc;
using JasperFx.Events.Daemon;

namespace Basket.Api.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart ShoppingCart) :ICommand<StoreBasketResult>;
public record StoreBasketResult(string UserName);
public class StoreBasketValidator:AbstractValidator<StoreBasketCommand>
{
    public StoreBasketValidator()
    {
        RuleFor(x => x.ShoppingCart).NotNull().WithMessage("Shopping cart cannot be null.");
        RuleFor(x => x.ShoppingCart.UserName).NotEmpty().WithMessage("User name is required.");
    }
}
internal class StoreBasketCommandHandler(IBasketRepository repository 
    , DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient) :ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        //Communicate with discount grpc and calculate the discount
        await DeductDiscount(command.ShoppingCart , cancellationToken); 

        await repository.StoreBasketAsync(command.ShoppingCart, cancellationToken);

        return new StoreBasketResult(command.ShoppingCart.UserName);
    }

    private async Task DeductDiscount(ShoppingCart shoppingCart, CancellationToken cancellationToken)
    {
        foreach (var item in shoppingCart.Items)
        {
            var coupon = await discountProtoServiceClient.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
            item.Price -= coupon.Amount;
        }
    }
}