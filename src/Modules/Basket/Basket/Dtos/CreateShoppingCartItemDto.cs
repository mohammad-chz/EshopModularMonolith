namespace Basket.Dtos;

public record CreateShoppingCartItemDto(
    Guid ProductId,
    int Quantity,
    string? Color,
    long Price, 
    string ProductName
    );
