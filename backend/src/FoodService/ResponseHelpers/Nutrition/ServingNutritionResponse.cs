namespace FoodService.ResponseHelpers.Nutrition;

/// <summary>
/// Nutrition facts calculated for a specific serving size
/// </summary>
public class ServingNutritionResponse
{
    /// <summary>
    /// Food ID
    /// </summary>
    public Guid FoodId { get; set; }

    /// <summary>
    /// Food name
    /// </summary>
    public string FoodName { get; set; } = string.Empty;

    /// <summary>
    /// Serving size in grams
    /// </summary>
    public decimal ServingGrams { get; set; }

    /// <summary>
    /// Serving description (e.g., "1 bát", "1 quả")
    /// </summary>
    public string ServingDescription { get; set; } = string.Empty;

    /// <summary>
    /// Multiplier from 100g base
    /// </summary>
    public decimal Multiplier { get; set; }

    // Calculated nutrition values for serving
    public decimal? CaloriesKcal { get; set; }
    public decimal? ProteinG { get; set; }
    public decimal? CarbsG { get; set; }
    public decimal? FatG { get; set; }
    public decimal? FiberG { get; set; }
    public decimal? SugarG { get; set; }
    public decimal? SodiumMg { get; set; }
    public decimal? CalciumMg { get; set; }
    public decimal? IronMg { get; set; }
    public decimal? VitaminCMg { get; set; }
    public decimal? VitaminAMcg { get; set; }

    // Additional nutrients
    public decimal? SaturatedFatG { get; set; }
    public decimal? CholesterolMg { get; set; }
    public decimal? PotassiumMg { get; set; }
    public decimal? MagnesiumMg { get; set; }
    public decimal? ZincMg { get; set; }
    public decimal? VitaminB6Mg { get; set; }
    public decimal? VitaminB12Mcg { get; set; }
    public decimal? FolateMcg { get; set; }

    // New nutrients from fixed DTOs
    public decimal? BiotinMcg { get; set; }
    public decimal? CholineMg { get; set; }
    public decimal? CopperMg { get; set; }
    public decimal? ManganeseMg { get; set; }
    public decimal? SeleniumMcg { get; set; }
    public decimal? PantothenicAcidMg { get; set; }
}
