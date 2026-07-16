using Basket.Basket.Models;

namespace Basket.Dtos;

public record CreateShoppingCartDto(
    string UserName,
    List<CreateShoppingCartItemDto> Items
    );
