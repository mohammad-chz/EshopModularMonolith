using Catalog.Data;
using Catalog.Products.Exceptions;
using Shared.CQRS;

namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductCommand(UpdateProductDto Dto) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

internal class UpdateProductHandler(CatalogDbContext context) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await context.Products.FindAsync([command.Dto.Id], cancellationToken) ?? throw new ProductNotFoundException(command.Dto.Id);

        UpdateProduct(product, command.Dto);

        context.Products.Update(product);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }

    private void UpdateProduct(Product product, UpdateProductDto dto)
    {
        product.Update(
           dto.Name,
           dto.Category,
           dto.Description,
           dto.ImageFile,
           dto.Price
           );
    }
}
