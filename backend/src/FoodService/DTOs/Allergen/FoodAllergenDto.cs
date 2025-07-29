namespace FoodService.DTOs.Allergen;

/// <summary>
/// Food-Allergen relationship DTO
/// </summary>
public class FoodAllergenDto
{
    public Guid FoodId { get; set; }
    public Guid AllergenId { get; set; }
    public AllergenDto Allergen { get; set; } = null!;
    public string SeverityLevel { get; set; } = "medium";
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}