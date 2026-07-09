namespace Basket.Dtos;

public record ShoppingCartItemDto(
    Guid Id,
    Guid ShoppingCartId,
    Guid ProductId,
    int Quantity,
    string? Color,
    long Price,
    string ProductName
    );
