namespace Basket.Dtos;

public record AddShoppingCartItem(
    string UserName,
    Guid ProductId,
    int Quantity,
    string? Color,
    long Price,
    string ProductName
    );
