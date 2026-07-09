namespace Basket.Basket.Features.DeleteBasket;

public record DeleteBasketResponse(bool IsSuccess);

internal class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}", async (
           string userName,
           ISender sender,
           CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new DeleteBasketCommand(userName), cancellationToken);

            var response = result.Adapt<DeleteBasketResponse>();

            return Results.Ok(response);
        })
       .WithName("DeleteBasket")
       .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
       .ProducesProblem(StatusCodes.Status400BadRequest)
       .ProducesProblem(StatusCodes.Status404NotFound)
       .WithSummary("Delete Basket")
       .WithDescription("Delete Basket");
    }
}
