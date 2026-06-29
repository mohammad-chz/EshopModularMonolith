namespace Catalog.Products.Exceptions
{
    internal class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(Guid id)
            : base($"Product with ID '{id}' was not found.")
        {
        }
    }
}
