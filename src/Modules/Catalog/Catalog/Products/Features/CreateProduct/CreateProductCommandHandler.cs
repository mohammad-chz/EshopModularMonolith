using Catalog.Data;
using Catalog.Dtos;
using Shared.CQRS;

namespace Catalog.Products.Features.CreateProduct;

public record CreateProductCommand(ProductDto Dto) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandHandler(CatalogDbContext context) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = CreateProduct(request);

        context.Products.Add(product);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }

    private static Product CreateProduct(CreateProductCommand request)
    {
        return Product.Create(
            request.Dto.Name,
            request.Dto.Category,
            request.Dto.Description,
            request.Dto.ImageFile,
            request.Dto.Price);
    }
}
