namespace Shared.Pagination
{
    public class PaginatedResult<TEntity>(int pageIndex, int pageSize, int count, IEnumerable<TEntity> data)
        where TEntity : class
    {
        public int PageIndex { get; set; } = Math.Max(1, pageIndex);
        public int PageSize { get; set; } = Math.Clamp(pageSize, 1, 100);
        public int Count { get; set; } = count;
        public IEnumerable<TEntity> Data { get; set; } = data;
    }
}
