using Catalog.Data;
using Catalog.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery() : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<ProductListDto> Dto);

internal class GetProductsHandler(CatalogDbContext context) : IQueryhandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var productsDto = await context.Products
            .OrderBy(p => p.Name)
            .ProjectToType<ProductListDto>()
            .ToListAsync(cancellationToken);
     
        return new GetProductsResult(productsDto);
    }
}
