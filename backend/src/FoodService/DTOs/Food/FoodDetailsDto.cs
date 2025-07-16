using FoodService.DTOs.Category;
using FoodService.DTOs.Allergen;
using FoodService.DTOs.Nutrition;

namespace FoodService.DTOs.Food;

/// <summary>
/// Complete food details DTO with all information
/// </summary>
public class FoodDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string NameVi { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? FoodCode { get; set; }
    public string Barcode { get; set; } = string.Empty;
    
    // Category information
    public Guid? CategoryId { get; set; }
    public CategoryDto? Category { get; set; }
    
    // Data source information
    public string DataSource { get; set; } = string.Empty;
    public string ExternalId { get; set; } = string.Empty;
    public string SourceUrl { get; set; } = string.Empty;
    
    // Serving size information
    public decimal? ServingSizeGrams { get; set; }
    public string ServingSizeDescription { get; set; } = string.Empty;
    public string ServingSizeDescriptionEn { get; set; } = string.Empty;
    public List<AlternativeServingDto> AlternativeServingSizes { get; set; } = new();
    
    // Nutrition information
    public NutritionFactsDto? NutritionFacts { get; set; }
    
    // Allergen information
    public List<AllergenDto> Allergens { get; set; } = new();
    
    // Status and verification
    public bool IsVerified { get; set; }
    public string VerificationStatus { get; set; } = "pending";
    public Guid? VerifiedBy { get; set; }
    public DateTime? VerifiedAt { get; set; }
    
    // Metadata
    public bool IsActive { get; set; } = true;
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Alternative serving size DTO
/// </summary>
public class AlternativeServingDto
{
    public decimal Grams { get; set; }
    public string Description { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
} 