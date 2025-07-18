using System.ComponentModel.DataAnnotations;

namespace FoodService.DTOs.Common;

/// <summary>
/// Request DTO for food search parameters
/// </summary>
public class FoodSearchRequest : PaginationRequest
{
    /// <summary>
    /// Search query string
    /// </summary>
    [StringLength(255, ErrorMessage = "Query không được vượt quá 255 ký tự")]
    public string Query { get; set; } = string.Empty;

    /// <summary>
    /// Category ID filter
    /// </summary>
    public Guid? CategoryId { get; set; }

    /// <summary>
    /// Data source filter
    /// </summary>
    public string? DataSource { get; set; }

    /// <summary>
    /// Verification status filter
    /// </summary>
    public string? VerificationStatus { get; set; }

    /// <summary>
    /// Only include verified foods
    /// </summary>
    public bool? OnlyVerified { get; set; }

    /// <summary>
    /// Only include active foods
    /// </summary>
    public bool? OnlyActive { get; set; } = true;

    /// <summary>
    /// Allergen IDs to exclude
    /// </summary>
    public List<Guid>? ExcludeAllergens { get; set; }

    /// <summary>
    /// Allergen IDs to include
    /// </summary>
    public List<Guid>? IncludeAllergens { get; set; }

    /// <summary>
    /// Minimum calories filter
    /// </summary>
    [Range(0, 5000, ErrorMessage = "Calories tối thiểu phải từ 0 đến 5000")]
    public decimal? MinCalories { get; set; }

    /// <summary>
    /// Maximum calories filter
    /// </summary>
    [Range(0, 5000, ErrorMessage = "Calories tối đa phải từ 0 đến 5000")]
    public decimal? MaxCalories { get; set; }

    /// <summary>
    /// Minimum protein filter (grams)
    /// </summary>
    [Range(0, 200, ErrorMessage = "Protein tối thiểu phải từ 0 đến 200g")]
    public decimal? MinProtein { get; set; }

    /// <summary>
    /// Maximum protein filter (grams)
    /// </summary>
    [Range(0, 200, ErrorMessage = "Protein tối đa phải từ 0 đến 200g")]
    public decimal? MaxProtein { get; set; }
} 