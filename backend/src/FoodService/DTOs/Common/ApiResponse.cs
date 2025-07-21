namespace FoodService.DTOs.Common;

/// <summary>
/// Standard API response wrapper
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();
    public Dictionary<string, List<string>> ValidationErrors { get; set; } = new();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string RequestId { get; set; } = Guid.NewGuid().ToString();

    // Success response
    public static ApiResponse<T> SuccessResponse(T data, string message = "Call API successfully.")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    // Error response
    public static ApiResponse<T> ErrorResponse(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors ?? []
        };
    }

    // Validation error response
    public static ApiResponse<T> ValidationErrorResponse(Dictionary<string, List<string>?> validationErrors)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = "Dữ liệu không hợp lệ",
            ValidationErrors = validationErrors
        };
    }
}

/// <summary>
/// Paginated response wrapper
/// </summary>
public class PaginatedResponse<T>
{
    public List<T> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
    public int? PreviousPageNumber => HasPreviousPage ? PageNumber - 1 : null;
    public int? NextPageNumber => HasNextPage ? PageNumber + 1 : null;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static PaginatedResponse<T> Create(List<T> items, int totalCount, int pageNumber, int pageSize)
    {
        return new PaginatedResponse<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}

/// <summary>
/// Search request DTO
/// </summary>
public class SearchRequest
{
    public string Query { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string SortBy { get; set; } = "UpdatedAt";
    public string SortOrder { get; set; } = "desc"; // "asc" or "desc"
    public Dictionary<string, object> Filters { get; set; } = new();
    public bool IncludeInactive { get; set; } = false;
}

/// <summary>
/// Search response DTO
/// </summary>
public class SearchResponse<T> : PaginatedResponse<T>
{
    public string Query { get; set; } = string.Empty;
    public double SearchTimeMs { get; set; }
    public Dictionary<string, int> Facets { get; set; } = new();
    public List<string> Suggestions { get; set; } = new();
    public bool HasMoreResults => TotalCount > (PageNumber * PageSize);
}

/// <summary>
/// Batch request DTO
/// </summary>
public class BatchRequest<T>
{
    public List<T> Items { get; set; } = new();
    public bool StopOnFirstError { get; set; } = false;
    public int BatchSize { get; set; } = 100;
}

/// <summary>
/// Batch response DTO
/// </summary>
public class BatchResponse<T>
{
    public List<T> SuccessItems { get; set; } = new();
    public List<BatchError> FailedItems { get; set; } = new();
    public int TotalProcessed { get; set; }
    public int SuccessCount { get; set; }
    public int FailureCount { get; set; }
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
    public double ProcessingTimeMs { get; set; }
}

/// <summary>
/// Batch error DTO
/// </summary>
public class BatchError
{
    public int Index { get; set; }
    public string Error { get; set; } = string.Empty;
    public Dictionary<string, List<string>> ValidationErrors { get; set; } = new();
    public object? OriginalItem { get; set; }
} 