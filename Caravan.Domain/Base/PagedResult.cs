namespace Caravan.Domain.Base;

public class PagedResult<T> where T : notnull
{
    public PagedResult()
    {
        Items = new List<T>();
    }
    
    public IList<T> Items { get; init; }
    public long Count { get; init; }
    public long PageNumber { get; init; }
    public long PageSize { get; init; }
    public long PageCount { get; init; }
    public long TotalItemCount { get; init; }
    public bool HasPreviousPage { get; init; }
    public bool HasNextPage { get; init; }
    public bool IsFirstPage { get; init; }
    public bool IsLastPage { get; init; }
    public long FirstItemOnPage { get; init; }
    public long LastItemOnPage { get; init; }
}