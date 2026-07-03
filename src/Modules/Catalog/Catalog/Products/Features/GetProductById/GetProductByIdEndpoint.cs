using Catalog.Products.Features.GetProductsByCategory;

namespace Catalog.Products.Features.GetProductById;

public record GetProductsByIdResponse(ProductListDto Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken
            ) =>
        {
            var query = await sender.Send(new GetProductByIdQuery(id), cancellationToken);

            var reslt = query.Adapt<GetProductsByIdResponse>();

            return Results.Ok(reslt);
        })
        .WithName("GetProductById")
        .Produces<GetProductsByIdResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)  
        .WithSummary("Get Product By Id")
        .WithDescription("Get Product By Id");
    }
}
