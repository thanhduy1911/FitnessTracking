using System.ComponentModel.DataAnnotations;

namespace FoodService.DTOs.Common;

/// <summary>
/// Request DTO for pagination parameters
/// </summary>
public class PaginationRequest
{
    /// <summary>
    /// Page number (1-based)
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Page phải lớn hơn 0")]
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of items per page
    /// </summary>
    [Range(1, 100, ErrorMessage = "PageSize phải từ 1 đến 100")]
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// Field to sort by
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Sort direction (asc/desc)
    /// </summary>
    public string SortDirection { get; set; } = "asc";

    /// <summary>
    /// Calculate skip count for database queries
    /// </summary>
    public int Skip => (Page - 1) * PageSize;
} 