namespace FoodService.ResponseHelpers.Nutrition;

/// <summary>
/// Daily value percentage for a nutrient
/// </summary>
public class DailyValuePercentageResponse
{
    /// <summary>
    /// Nutrient name
    /// </summary>
    public string NutrientName { get; set; } = string.Empty;

    /// <summary>
    /// Vietnamese nutrient name
    /// </summary>
    public string NutrientNameVi { get; set; } = string.Empty;

    /// <summary>
    /// Nutrient value
    /// </summary>
    public decimal NutrientValue { get; set; }

    /// <summary>
    /// Nutrient unit (g, mg, mcg, etc.)
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// Daily value reference amount
    /// </summary>
    public decimal DailyValueReference { get; set; }

    /// <summary>
    /// Percentage of daily value
    /// </summary>
    public decimal Percentage { get; set; }

    /// <summary>
    /// Percentage category
    /// </summary>
    public string Category { get; set; } = string.Empty; // "low", "medium", "high", "very_high"

    /// <summary>
    /// Vietnamese category description
    /// </summary>
    public string CategoryVi { get; set; } = string.Empty; // "thấp", "trung bình", "cao", "rất cao"

    /// <summary>
    /// Health recommendation
    /// </summary>
    public string? HealthRecommendation { get; set; }

    /// <summary>
    /// Vietnamese health recommendation
    /// </summary>
    public string? HealthRecommendationVi { get; set; }
}
