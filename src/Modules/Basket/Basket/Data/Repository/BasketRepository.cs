using Basket.Basket.Models;
using Microsoft.EntityFrameworkCore;

namespace Basket.Data.Repository
{
    internal class BasketRepository(BasketDbContext context) : IBasketRepository
    {
        public async Task<ShoppingCart?> GetBasket(string userName, bool asNoTracking = true, CancellationToken ct = default)
        {
            var normalized = userName.Trim().ToLower();

            var query = context.ShoppingCarts
                .Include(sh => sh.Items)
                .Where(sh => sh.UserName == normalized);

            if (asNoTracking)
                query = query.AsNoTracking();

            return await query.SingleOrDefaultAsync(ct);
        }

        public async Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken ct = default)
        {
            context.ShoppingCarts.Add(basket);
            await context.SaveChangesAsync(ct);

            return basket;
        }

        public async Task<bool> DeleteBasket(ShoppingCart basket, CancellationToken ct = default)
        {
            context.Remove(basket);
            await context.SaveChangesAsync(ct);

            return true;
        }

        public IQueryable<ShoppingCart> Query()
        {
            return context.ShoppingCarts;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await context.SaveChangesAsync(ct);
        }
    }
}
