using Shared.DDD;
using System.Drawing;

namespace Basket.Basket.Models
{
    public class ShoppingCartItem : Entity<Guid>
    {
        public Guid ShoppingCartId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public string? Color { get; private set; }

        public long Price { get; set; }
        public string ProductName { get; set; }

        internal ShoppingCartItem(Guid shoppingCartId, Guid productId, int quantity, string? color, long price, string productName)
        {
            ShoppingCartId = shoppingCartId;
            ProductId = productId;
            Quantity = quantity;
            Color = color;
            Price = price;
            ProductName = productName;
        }

        internal void IncreaseQuantity(int quantity) => Quantity += quantity;
    }
}
