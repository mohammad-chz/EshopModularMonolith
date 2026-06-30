using Catalog.Data;
using Catalog.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Catalog.Products.Features.GetProductsByCategory;

public record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;

public record GetProductsByCategoryResult(IEnumerable<ProductListDto> Dto);

internal class GetProductsByCategoryHandler(CatalogDbContext context) : IQueryhandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var prodctsDto = await context.Products
            .OrderBy(p => p.Name)
            .ProjectToType<ProductListDto>()
            .ToListAsync(cancellationToken);

        return new GetProductsByCategoryResult(prodctsDto);
    }
}
