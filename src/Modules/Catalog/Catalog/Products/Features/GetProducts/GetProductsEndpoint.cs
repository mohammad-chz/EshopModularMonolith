using Catalog.Products.Features.CreateProduct;

namespace Catalog.Products.Features.GetProducts;

public record GetProductsResponse(IEnumerable<ProductListDto> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = await sender.Send(new GetProductsQuery(), cancellationToken);

            var reslt = query.Adapt<GetProductsResponse>();

            return Results.Ok(reslt);
        })
        .WithName("GetAllProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Get All Product")
        .WithDescription("Get All Product");
    }
}
