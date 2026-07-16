using Basket.Basket.Models;

namespace Basket.Data.Repository
{
    internal interface IBasketRepository
    {
        Task<ShoppingCart?> GetBasket(string userName, bool asNoTracking = true, CancellationToken ct = default);
        Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken ct = default);
        Task<bool> DeleteBasket(ShoppingCart cart, CancellationToken ct = default);
        IQueryable<ShoppingCart> Query();

        Task<int> SaveChangesAsync(CancellationToken ct =  default);
    }
}
