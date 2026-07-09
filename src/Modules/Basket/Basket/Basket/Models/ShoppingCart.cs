using Shared.DDD;
using Shared.Exceptions;

namespace Basket.Basket.Models
{
    public class ShoppingCart : Aggregate<Guid>
    {
        public string UserName { get; private set; } = default!;

        private readonly List<ShoppingCartItem> _items = [];
        public IReadOnlyList<ShoppingCartItem> Items => _items.AsReadOnly();
        public long TotalPrice => _items.Sum(x => x.Price * x.Quantity);

        private ShoppingCart(Guid id, string userName)
        {
            Id = id;
            UserName = userName;
        }

        public static ShoppingCart Create(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new DomainException("Username cannot be empty");

            var cart = new ShoppingCart(SequentialGuid.NewId(), userName);

            return cart;
        }

        public void AddItem(Guid productId, int quantity, string? color, long price, string productName)
        {
            if (quantity <= 0)
                throw new DomainException("Quantity must be greater than zero");

            if (price <= 0)
                throw new DomainException("price must be greater than zero");

            var existing = _items.FirstOrDefault(x => x.ProductId == productId);

            if (existing != null)
                existing.IncreaseQuantity(quantity);
            else
                _items.Add(new ShoppingCartItem(Id, productId, quantity, color, price, productName));

        }

        public void RemoveItem(Guid productId)
        {
            var existing = _items.FirstOrDefault(x => x.ProductId == productId);

            if (existing == null)
                return;

            _items.Remove(existing);
        }
    }
}
