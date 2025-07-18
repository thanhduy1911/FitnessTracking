using System.ComponentModel.DataAnnotations;

namespace FoodService.RequestHelpers.Common;

/// <summary>
/// Search request with validation
/// </summary>
public class SearchRequest
{
    [StringLength(100, ErrorMessage = "Query không được vượt quá 100 ký tự")]
    public string Query { get; set; } = string.Empty;

    [Range(1, 1000, ErrorMessage = "PageNumber phải từ 1 đến 1000")]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100, ErrorMessage = "PageSize phải từ 1 đến 100")]
    public int PageSize { get; set; } = 20;

    [StringLength(50, ErrorMessage = "SortBy không được vượt quá 50 ký tự")]
    public string SortBy { get; set; } = "UpdatedAt";

    [RegularExpression(@"^(asc|desc)$", ErrorMessage = "SortOrder phải là 'asc' hoặc 'desc'")]
    public string SortOrder { get; set; } = "desc"; // "asc" or "desc"

    public Dictionary<string, object> Filters { get; set; } = new();

    public bool IncludeInactive { get; set; } = false;
}
