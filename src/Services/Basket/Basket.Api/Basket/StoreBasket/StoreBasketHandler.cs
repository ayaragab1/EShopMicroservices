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
internal class StoreBasketCommandHandler(IBasketRepository repository) :ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
       await repository.StoreBasketAsync(command.ShoppingCart, cancellationToken);

       return new StoreBasketResult(command.ShoppingCart.UserName);
    }
}