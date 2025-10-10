namespace Basket.Api.Data;

public class BasketRepository(IDocumentSession session) : IBasketRepository
{
    public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellation = default)
    {
        var basket = await session.LoadAsync<ShoppingCart>(userName, cancellation);
        if (basket is null)
            throw new BasketNotFoundException(userName); 
        return basket;
    }

    public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart cart, CancellationToken cancellation = default)
    { 
        session.Store(cart);
        await session.SaveChangesAsync(cancellation);
        return cart;
    }

    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellation = default)
    {
        session.Delete<ShoppingCart>(userName);
        await session.SaveChangesAsync(cancellation);
        return true;
    }
}