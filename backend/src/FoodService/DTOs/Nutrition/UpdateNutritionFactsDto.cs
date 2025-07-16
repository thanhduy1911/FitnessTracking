using System.ComponentModel.DataAnnotations;

namespace FoodService.DTOs.Nutrition;

/// <summary>
/// DTO for updating nutrition facts
/// </summary>
public class UpdateNutritionFactsDto
{
    // Macronutrients per 100g
    [Range(0, 2000, ErrorMessage = "Calories phải từ 0 đến 2000 kcal")]
    public decimal? CaloriesKcal { get; set; }
    
    [Range(0, 200, ErrorMessage = "Protein phải từ 0 đến 200g")]
    public decimal? ProteinG { get; set; }
    
    [Range(0, 200, ErrorMessage = "Carbohydrates phải từ 0 đến 200g")]
    public decimal? CarbsG { get; set; }
    
    [Range(0, 200, ErrorMessage = "Fat phải từ 0 đến 200g")]
    public decimal? FatG { get; set; }
    
    [Range(0, 100, ErrorMessage = "Fiber phải từ 0 đến 100g")]
    public decimal? FiberG { get; set; }
    
    [Range(0, 200, ErrorMessage = "Sugar phải từ 0 đến 200g")]
    public decimal? SugarG { get; set; }
    
    [Range(0, 10000, ErrorMessage = "Sodium phải từ 0 đến 10000mg")]
    public decimal? SodiumMg { get; set; }
    
    // Detailed fat breakdown
    [Range(0, 200, ErrorMessage = "Saturated fat phải từ 0 đến 200g")]
    public decimal? SaturatedFatG { get; set; }
    
    [Range(0, 200, ErrorMessage = "Monounsaturated fat phải từ 0 đến 200g")]
    public decimal? MonounsaturatedFatG { get; set; }
    
    [Range(0, 200, ErrorMessage = "Polyunsaturated fat phải từ 0 đến 200g")]
    public decimal? PolyunsaturatedFatG { get; set; }
    
    [Range(0, 200, ErrorMessage = "Trans fat phải từ 0 đến 200g")]
    public decimal? TransFatG { get; set; }
    
    [Range(0, 1000, ErrorMessage = "Cholesterol phải từ 0 đến 1000mg")]
    public decimal? CholesterolMg { get; set; }
    
    // Minerals (per 100g)
    [Range(0, 10000, ErrorMessage = "Calcium phải từ 0 đến 10000mg")]
    public decimal? CalciumMg { get; set; }
    
    [Range(0, 1000, ErrorMessage = "Iron phải từ 0 đến 1000mg")]
    public decimal? IronMg { get; set; }
    
    [Range(0, 1000, ErrorMessage = "Magnesium phải từ 0 đến 1000mg")]
    public decimal? MagnesiumMg { get; set; }
    
    [Range(0, 10000, ErrorMessage = "Phosphorus phải từ 0 đến 10000mg")]
    public decimal? PhosphorusMg { get; set; }
    
    [Range(0, 10000, ErrorMessage = "Potassium phải từ 0 đến 10000mg")]
    public decimal? PotassiumMg { get; set; }
    
    [Range(0, 1000, ErrorMessage = "Zinc phải từ 0 đến 1000mg")]
    public decimal? ZincMg { get; set; }
    
    // Vitamins (per 100g)
    [Range(0, 10000, ErrorMessage = "Vitamin A phải từ 0 đến 10000mcg")]
    public decimal? VitaminAMcg { get; set; }
    
    [Range(0, 1000, ErrorMessage = "Vitamin B1 phải từ 0 đến 1000mg")]
    public decimal? VitaminB1Mg { get; set; }
    
    [Range(0, 1000, ErrorMessage = "Vitamin B2 phải từ 0 đến 1000mg")]
    public decimal? VitaminB2Mg { get; set; }
    
    [Range(0, 1000, ErrorMessage = "Vitamin B3 phải từ 0 đến 1000mg")]
    public decimal? VitaminB3Mg { get; set; }
    
    [Range(0, 1000, ErrorMessage = "Vitamin B6 phải từ 0 đến 1000mg")]
    public decimal? VitaminB6Mg { get; set; }
    
    [Range(0, 10000, ErrorMessage = "Vitamin B12 phải từ 0 đến 10000mcg")]
    public decimal? VitaminB12Mcg { get; set; }
    
    [Range(0, 10000, ErrorMessage = "Vitamin C phải từ 0 đến 10000mg")]
    public decimal? VitaminCMg { get; set; }
    
    [Range(0, 10000, ErrorMessage = "Vitamin D phải từ 0 đến 10000mcg")]
    public decimal? VitaminDMcg { get; set; }
    
    [Range(0, 1000, ErrorMessage = "Vitamin E phải từ 0 đến 1000mg")]
    public decimal? VitaminEMg { get; set; }
    
    [Range(0, 10000, ErrorMessage = "Vitamin K phải từ 0 đến 10000mcg")]
    public decimal? VitaminKMcg { get; set; }
    
    [Range(0, 10000, ErrorMessage = "Folate phải từ 0 đến 10000mcg")]
    public decimal? FolateMcg { get; set; }
    
    // Data quality indicators
    [StringLength(20, ErrorMessage = "Data quality không được vượt quá 20 ký tự")]
    public string? DataQuality { get; set; }
    
    [StringLength(100, ErrorMessage = "Data source không được vượt quá 100 ký tự")]
    public string? DataSource { get; set; }
    
    public DateTime? LastVerified { get; set; }
    public string? Notes { get; set; }
} 