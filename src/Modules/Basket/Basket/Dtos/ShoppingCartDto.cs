using Basket.Basket.Models;

namespace Basket.Dtos;

public record ShoppingCartDto(
    Guid Id,
    string UserName,
    IReadOnlyList<ShoppingCartItemDto> Items
    );
