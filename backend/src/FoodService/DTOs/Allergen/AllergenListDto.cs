namespace FoodService.DTOs.Allergen;

/// <summary>
/// Lightweight allergen DTO for listings
/// </summary>
public class AllergenListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameVi { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public string? ColorHex { get; set; }
    public string Category { get; set; } = string.Empty;
    public string SeverityLevel { get; set; } = "medium";
    public bool IsCommonInVietnameseFood { get; set; }
    public bool IsActive { get; set; } = true;
}