using Catalog.Data;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;
using Shared.Pagination;

namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery(PaginationRequest Pagination) : IQuery<GetProductsResult>;

public record GetProductsResult(PaginatedResult<ProductListDto> Products);

internal class GetProductsHandler(CatalogDbContext context) : IQueryhandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = Math.Max(1, query.Pagination.PageIndex);
        var pageSize = Math.Clamp(query.Pagination.PageSize, 1, 100);

        var baseQuery = context.Products.AsNoTracking();

        var count = await baseQuery.CountAsync(cancellationToken);
        if (count == 0)
            return new GetProductsResult(new PaginatedResult<ProductListDto>(pageIndex, pageSize, 0, []));

        var products = await baseQuery
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Skip(pageSize * (pageIndex - 1))
            .Take(pageSize)
            .ProjectToType<ProductListDto>()
            .ToListAsync(cancellationToken);

        return new GetProductsResult(new PaginatedResult<ProductListDto>(pageIndex, pageSize, count, products));
    }
}
