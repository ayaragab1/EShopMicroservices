using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Api.Data;

public class CacheBasketRepository (IBasketRepository repository, IDistributedCache cache):IBasketRepository
{
    public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellation = default)
    {
        var cachedBasket = await cache.GetStringAsync(userName, cancellation);
        if (!string.IsNullOrEmpty(cachedBasket))
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
        
        var basket= await repository.GetBasketAsync(userName, cancellation);
        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellation);
        return basket; 
    }

    public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart cart, CancellationToken cancellation = default)
    {
         await repository.StoreBasketAsync(cart, cancellation);
         await cache.SetStringAsync(cart.UserName, JsonSerializer.Serialize(cart), cancellation);

         return cart; 
    }

    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellation = default)
    {
         await repository.DeleteBasketAsync(userName, cancellation);
         await cache.RemoveAsync(userName, cancellation);

         return true; 
    }
}