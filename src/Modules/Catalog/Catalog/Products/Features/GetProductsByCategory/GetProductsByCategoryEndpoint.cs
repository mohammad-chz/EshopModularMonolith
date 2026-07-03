using Catalog.Products.Features.CreateProduct;
using Catalog.Products.Features.GetProducts;

namespace Catalog.Products.Features.GetProductsByCategory;

public record GetProductsByCategoryResponse(IEnumerable<ProductListDto> Products);
public class GetProductsByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category", async (
            string category,
            ISender sender,
            CancellationToken cancellationToken
            ) =>
        {
            var query = await sender.Send(new GetProductsByCategoryQuery(category), cancellationToken);

            var reslt = query.Adapt<GetProductsByCategoryResponse>();

            return Results.Ok(reslt);
        })
        .WithName("GetProductsByCategory")
        .Produces<GetProductsByCategoryEndpoint>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products By Category")
        .WithDescription("Get Products By Category");
    }
}
