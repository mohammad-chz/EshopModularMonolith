namespace Catalog.Products.Features.CreateProduct;

public record CreateProductRequest(CreateProductDto Product);

public record CreateProductResponse(Guid Id);

internal class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (
            CreateProductRequest request, 
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<CreateProductCommand>();

            var result = await sender.Send(command, cancellationToken);

            var response = result.Adapt<CreateProductResponse>();

            return Results.Created($"/products/{response.Id}", response);
        })
        .WithName("CreateProduct")
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Product")
        .WithDescription("Create Product");
    }
}
