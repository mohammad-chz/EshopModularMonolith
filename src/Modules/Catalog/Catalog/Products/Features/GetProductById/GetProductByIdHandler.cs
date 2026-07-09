using Catalog.Data;
using Catalog.Products.Exceptions;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Catalog.Products.Features.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(ProductListDto Product);

internal class GetProductByIdHandler(CatalogDbContext context) : IQueryhandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await context.Products
            .AsNoTracking()
            .Where(p => p.Id == query.Id)
            .ProjectToType<ProductListDto>()
            .SingleOrDefaultAsync(cancellationToken) ?? throw new ProductNotFoundException(query.Id);

        return new GetProductByIdResult(product);
    }
}
