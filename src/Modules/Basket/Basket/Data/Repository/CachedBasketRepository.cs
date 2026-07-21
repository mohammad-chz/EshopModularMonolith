using Basket.Basket.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Data.Repository
{
    public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
    {
        public async Task<ShoppingCart?> GetBasket(string userName, bool asNoTracking = true, CancellationToken ct = default)
        {
            if (!asNoTracking)
                return await repository.GetBasket(userName, asNoTracking, ct);

            var cachedBasket = await cache.GetStringAsync(userName, ct);
            if (!string.IsNullOrWhiteSpace(cachedBasket))
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket);

            var basket = await repository.GetBasket(userName, asNoTracking, ct);

            if (basket is not null)
                await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), ct);

            return basket;
        }

        public async Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken ct = default)
        {
            var cart = await repository.CreateBasket(basket, ct);

            await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(cart), ct);

            return cart;
        }

        public async Task<bool> DeleteBasket(ShoppingCart cart, CancellationToken ct = default)
        {
            await repository.DeleteBasket(cart, ct);

            await cache.RemoveAsync(cart.UserName, ct);

            return true;
        }

        public async Task<int> SaveChangesAsync(string? userName = null, CancellationToken ct = default)
        {
            var result = await repository.SaveChangesAsync(userName, ct);

            if (userName is not null)
                await cache.RemoveAsync(userName, ct);

            return result;
        }
    }
}
