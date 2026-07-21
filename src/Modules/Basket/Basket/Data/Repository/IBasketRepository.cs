using Basket.Basket.Models;

namespace Basket.Data.Repository
{
    public interface IBasketRepository
    {
        Task<ShoppingCart?> GetBasket(string userName, bool asNoTracking = true, CancellationToken ct = default);
        Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken ct = default);
        Task<bool> DeleteBasket(ShoppingCart cart, CancellationToken ct = default);

        Task<int> SaveChangesAsync(string? userName = null, CancellationToken ct = default);
    }
}
