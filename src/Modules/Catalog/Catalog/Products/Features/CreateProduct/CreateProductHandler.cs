using Catalog.Data;
using Catalog.Dtos;
using Shared.CQRS;

namespace Catalog.Products.Features.CreateProduct;

public record CreateProductCommand(CreateProductDto Dto) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

internal class CreateProductHandler(CatalogDbContext context) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = CreateProduct(command.Dto);

        context.Products.Add(product);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }

    private static Product CreateProduct(CreateProductDto command)
    {
        return Product.Create(
            command.Name,
            command.Category,
            command.Description,
            command.ImageFile,
            command.Price);
    }
}
