using Shared.Exceptions;

namespace Catalog.Products.Exceptions
{
    internal sealed class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(Guid id)
            : base("product", id)
        {
        }
    }
}
