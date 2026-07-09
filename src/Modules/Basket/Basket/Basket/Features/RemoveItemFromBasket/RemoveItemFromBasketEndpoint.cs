using Basket.Basket.Features.DeleteBasket;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Basket.Features.RemoveItemFromBasket;

public record RemoveItemFromBasketResponse(bool IsSuccess);

internal class RemoveItemFromBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}/items/{productId}", async (
           [FromRoute] string userName,
           [FromRoute] Guid productId,
           ISender sender,
           CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new RemoveItemFromBasketCommand(userName, productId), cancellationToken);

            var response = result.Adapt<RemoveItemFromBasketResponse>();

            return Results.Ok(response);
        })
       .WithName("RemoveShoppingCartItem")
       .Produces<RemoveItemFromBasketResponse>(StatusCodes.Status200OK)
       .ProducesProblem(StatusCodes.Status400BadRequest)
       .ProducesProblem(StatusCodes.Status404NotFound)
       .WithSummary("Remove shopping cart item")
       .WithDescription("Remove an item from the shopping cart");
    }
}
