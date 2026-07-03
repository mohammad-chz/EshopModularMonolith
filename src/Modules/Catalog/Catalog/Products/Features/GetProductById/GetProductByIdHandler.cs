using Catalog.Data;
using Catalog.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Catalog.Products.Features.GetProductById;

public record GetProductByIdQuery(Guid Id) : ICommand<GetProductByIdResult>;

public record GetProductByIdResult(ProductListDto Product);

internal class GetProductByIdHandler(CatalogDbContext context) : ICommandHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await context.Products
            .ProjectToType<ProductListDto>()
            .SingleAsync(p => p.Id == query.Id, cancellationToken);

        return new GetProductByIdResult(product);
    }
}
