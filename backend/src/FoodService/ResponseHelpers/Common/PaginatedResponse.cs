namespace FoodService.ResponseHelpers.Common;

/// <summary>
/// Paginated response wrapper
/// </summary>
public class PaginatedResponse<T>
{
    public List<T> Data { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
    public int? PreviousPageNumber => HasPreviousPage ? PageNumber - 1 : null;
    public int? NextPageNumber => HasNextPage ? PageNumber + 1 : null;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static PaginatedResponse<T> Create(List<T> data, int totalCount, int pageNumber, int pageSize)
    {
        return new PaginatedResponse<T>
        {
            Data = data,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}
