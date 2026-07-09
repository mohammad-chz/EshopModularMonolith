using Shared.Pagination;

namespace Catalog.Products.Features.GetProducts;

public record GetProductsRequest(PaginationRequest Pagination);

public record GetProductsResponse(PaginatedResult<ProductListDto> Products);


public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (
            [AsParameters] PaginationRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetProductsQuery(request), cancellationToken);

            var response = result.Adapt<GetProductsResponse>();

            return Results.Ok(response);
        })
        .WithName("GetAllProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Get All Product")
        .WithDescription("Get All Product");
    }
}
