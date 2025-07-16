namespace FoodService.DTOs.Nutrition;

/// <summary>
/// Complete nutrition facts DTO
/// </summary>
public class NutritionFactsDto
{
    public Guid Id { get; set; }
    public Guid FoodId { get; set; }
    
    // Macronutrients per 100g
    public decimal? CaloriesKcal { get; set; }
    public decimal? ProteinG { get; set; }
    public decimal? CarbsG { get; set; }
    public decimal? FatG { get; set; }
    public decimal? FiberG { get; set; }
    public decimal? SugarG { get; set; }
    public decimal? SodiumMg { get; set; }
    
    // Detailed fat breakdown
    public decimal? SaturatedFatG { get; set; }
    public decimal? MonounsaturatedFatG { get; set; }
    public decimal? PolyunsaturatedFatG { get; set; }
    public decimal? TransFatG { get; set; }
    public decimal? CholesterolMg { get; set; }
    
    // Minerals (per 100g)
    public decimal? CalciumMg { get; set; }
    public decimal? IronMg { get; set; }
    public decimal? MagnesiumMg { get; set; }
    public decimal? PhosphorusMg { get; set; }
    public decimal? PotassiumMg { get; set; }
    public decimal? ZincMg { get; set; }
    
    // Vitamins (per 100g)
    public decimal? VitaminAMcg { get; set; }
    public decimal? VitaminB1Mg { get; set; }
    public decimal? VitaminB2Mg { get; set; }
    public decimal? VitaminB3Mg { get; set; }
    public decimal? VitaminB6Mg { get; set; }
    public decimal? VitaminB12Mcg { get; set; }
    public decimal? VitaminCMg { get; set; }
    public decimal? VitaminDMcg { get; set; }
    public decimal? VitaminEMg { get; set; }
    public decimal? VitaminKMcg { get; set; }
    public decimal? FolateMcg { get; set; }
    
    // Data quality indicators
    public string DataQuality { get; set; } = "medium";
    public string? DataSource { get; set; }
    public DateTime? LastVerified { get; set; }
    public string? Notes { get; set; }
    
    // Metadata
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
} 