namespace Basket.Api.Data;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasketAsync(string userName , CancellationToken cancellation = default);
    Task<ShoppingCart> StoreBasketAsync(ShoppingCart cart , CancellationToken cancellation = default);
    Task<bool> DeleteBasketAsync(string userName , CancellationToken cancellation = default);
}