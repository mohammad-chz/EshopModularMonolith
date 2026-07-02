using Catalog.Data;
using Catalog.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Catalog.Products.Features.GetProductById;

public record GetProductByIdHandlerQuery(Guid Id) : ICommand<GetProductByIdResult>;

public record GetProductByIdResult(ProductListDto Product);

internal class GetProductByIdHandler(CatalogDbContext context) : ICommandHandler<GetProductByIdHandlerQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdHandlerQuery query, CancellationToken cancellationToken)
    {
        var product = await context.Products
            .ProjectToType<ProductListDto>()
            .SingleAsync(p => p.Id == query.Id, cancellationToken);

        return new GetProductByIdResult(product);
    }
}
