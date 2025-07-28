namespace FoodService.DTOs.Allergen;

/// <summary>
/// DTO for displaying allergen information
/// </summary>
public class AllergenDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string NameVi { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public string? ColorHex { get; set; }
    
    // Allergen classification
    public string Category { get; set; } = string.Empty; // "protein", "grain", "dairy", "fruit", etc.
    public string SeverityLevel { get; set; } = "medium"; // "low", "medium", "high"
    
    // Common in Vietnamese foods
    public bool IsCommonInVietnameseFood { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int FoodCount { get; set; } // Số lượng món ăn chứa allergen này
}