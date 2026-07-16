using Basket.Basket.Models;
using System.Linq.Expressions;

namespace Basket.Basket.Features.GetBasket
{
    internal static class Projections
    {
        public static Expression<Func<ShoppingCart, ShoppingCartDto>> ToDto =>
            cart => new ShoppingCartDto(
                cart.Id,
                cart.UserName,
                cart.Items.Select(item => new ShoppingCartItemDto(
                    item.Id,
                    item.ShoppingCartId,
                    item.ProductId,
                    item.Color,
                    item.Quantity,
                    item.Price,
                    item.ProductName))
                .ToList());
    }
}
