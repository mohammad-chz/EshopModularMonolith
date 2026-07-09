namespace Shared.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string resourceName, object? key)
            : base($"{resourceName} with identifier '{key}' was not found.")
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }
    }
}
