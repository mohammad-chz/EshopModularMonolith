namespace Basket.Basket.Features.CreateBasket;

public record CreateBasketRequest(CreateShoppingCartDto ShoppingCart);

public record CreateBasketResponse(Guid Id);

internal class CreateBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (
           CreateBasketRequest request,
           ISender sender,
           CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<CreateBasketCommand>();

            var result = await sender.Send(command, cancellationToken);

            var response = result.Adapt<CreateBasketResponse>();

            return Results.CreatedAtRoute($"GetBasket", new { request.ShoppingCart.UserName }, response);
        })
       .WithName("CreateBasket")
       .Produces<CreateBasketResponse>(StatusCodes.Status201Created)
       .ProducesProblem(StatusCodes.Status400BadRequest)
       .WithSummary("Create Basket")
       .WithDescription("Create Basket");
    }
}
