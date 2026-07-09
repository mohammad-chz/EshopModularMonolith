
using Shared.Exceptions;

namespace Catalog.Products.Models
{
    public class Product : Aggregate<Guid>
    {
        public string Name { get; private set; } = default!;
        public List<string> Category { get; private set; } = [];
        public string? Description { get; private set; }
        public string? ImageFile { get; private set; }
        public decimal Price { get; private set; }

        public static Product Create(string name, List<string> category, string? description, string? imageFile, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Product name cannot be empty.");

            if (price <= 0)
                throw new DomainException("Product price must be greater than zero.");

            var product = new Product
            {
                Id = SequentialGuid.NewId(),
                Name = name,
                Category = category,
                Description = description,
                ImageFile = imageFile,
                Price = price
            };

            product.AddDomainEvent(new ProductCreatedEvent(product));

            return product;
        }

        public void Update(string name, List<string> category, string? description, string? imageFile, decimal price)
        {
            ArgumentException.ThrowIfNullOrEmpty(name);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            Name = name;
            Category = [.. category];
            Description = description;
            ImageFile = imageFile;

            if (Price != price)
            {
                Price = price;
                AddDomainEvent(new ProductPriceChangedEvent(this));
            }
        }
    }
}
