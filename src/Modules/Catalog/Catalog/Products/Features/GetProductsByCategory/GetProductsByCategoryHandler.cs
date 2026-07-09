using Catalog.Data;
using Catalog.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Catalog.Products.Features.GetProductsByCategory;

public record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;

public record GetProductsByCategoryResult(IEnumerable<ProductListDto> Products);

internal class GetProductsByCategoryHandler(CatalogDbContext context) : IQueryhandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        var prodctsDto = await context.Products
            .AsNoTracking()
            .Where(p => p.Category.Contains(query.Category))
            .OrderBy(p => p.Name)
            .ProjectToType<ProductListDto>()
            .ToListAsync(cancellationToken);

        return new GetProductsByCategoryResult(prodctsDto);
    }
}
