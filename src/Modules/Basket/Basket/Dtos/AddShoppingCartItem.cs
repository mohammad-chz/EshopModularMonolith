namespace Basket.Dtos;

public record AddShoppingCartItem(
    Guid ProductId,
    int Quantity,
    string? Color,
    long Price,
    string ProductName
    );
