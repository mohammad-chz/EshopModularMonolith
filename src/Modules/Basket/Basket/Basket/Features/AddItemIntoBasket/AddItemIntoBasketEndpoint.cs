using Basket.Basket.Features.CreateBasket;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketRequest(AddShoppingCartItem ShoppingCartItem);

public record AddItemIntoBasketResponse(Guid Id);

internal class AddItemIntoBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/{userName}/items", async (
          [FromRoute] string userName,
          [FromBody] AddItemIntoBasketRequest request,
          ISender sender,
          CancellationToken cancellationToken) =>
        {
            var command = new AddItemIntoBasketCommand(userName, request.ShoppingCartItem);

            var result = await sender.Send(command, cancellationToken);

            var response = result.Adapt<AddItemIntoBasketResponse>();

            return Results.CreatedAtRoute($"GetBasket", new { userName }, response);
        })
      .WithName("AddItemIntoBasket")
      .Produces<AddItemIntoBasketResponse>(StatusCodes.Status201Created)
      .ProducesProblem(StatusCodes.Status400BadRequest)
      .WithSummary("Add item into basket")
      .WithDescription("Add item into basket");
    }
}
