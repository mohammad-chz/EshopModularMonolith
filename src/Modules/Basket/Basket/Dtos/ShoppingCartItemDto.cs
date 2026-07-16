namespace Basket.Dtos;

public record ShoppingCartItemDto(
    Guid Id,
    Guid ShoppingCartId,
    Guid ProductId,
    string? Color,
    int Quantity,
    long Price,
    string ProductName
    );
