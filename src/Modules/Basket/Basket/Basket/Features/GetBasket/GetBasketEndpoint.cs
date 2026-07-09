namespace Basket.Basket.Features.GetBasket;

public record GetBasketResponse(ShoppingCartDto ShoppingCart);

internal class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}", async (
           string userName,
           ISender sender,
           CancellationToken cancellationToken
           ) =>
        {
            var query = await sender.Send(new GetBasketQuery(userName), cancellationToken);

            var result = query.Adapt<GetBasketResponse>();

            return Results.Ok(result);
        })
       .WithName("GetBasket")
       .Produces<GetBasketResponse>(StatusCodes.Status200OK)
       .Produces(StatusCodes.Status400BadRequest)
       .WithSummary("Get Basket By Id")
       .WithDescription("Get Basket By Id");
    }
}
