using Shared.Exceptions;

namespace Basket.Basket.Exceptions
{
    internal class BasketNotFoundException : NotFoundException
    {
        public BasketNotFoundException(string key) 
            : base("basket", key)
        {
        }
    }
}
