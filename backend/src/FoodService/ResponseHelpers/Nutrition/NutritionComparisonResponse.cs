namespace FoodService.ResponseHelpers.Nutrition;

/// <summary>
/// Nutrition comparison between multiple foods
/// </summary>
public class NutritionComparisonResponse
{
    /// <summary>
    /// List of foods being compared
    /// </summary>
    public List<FoodComparisonItem> Foods { get; set; } = new();

    /// <summary>
    /// Comparison summary
    /// </summary>
    public ComparisonSummary Summary { get; set; } = new();
}

/// <summary>
/// Individual food item in comparison
/// </summary>
public class FoodComparisonItem
{
    public Guid FoodId { get; set; }
    public string FoodName { get; set; } = string.Empty;
    public string FoodNameVi { get; set; } = string.Empty;
    public decimal ServingGrams { get; set; }
    public string ServingDescription { get; set; } = string.Empty;

    // Nutrition values
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

    // Rankings for this food
    public Dictionary<string, int> Rankings { get; set; } = new();
}

/// <summary>
/// Summary of nutrition comparison
/// </summary>
public class ComparisonSummary
{
    /// <summary>
    /// Food with highest calories
    /// </summary>
    public string? HighestCaloriesFood { get; set; }

    /// <summary>
    /// Food with highest protein
    /// </summary>
    public string? HighestProteinFood { get; set; }

    /// <summary>
    /// Food with lowest calories
    /// </summary>
    public string? LowestCaloriesFood { get; set; }

    /// <summary>
    /// Food with lowest fat
    /// </summary>
    public string? LowestFatFood { get; set; }

    /// <summary>
    /// Average calories across all foods
    /// </summary>
    public decimal AverageCalories { get; set; }

    /// <summary>
    /// Average protein across all foods
    /// </summary>
    public decimal AverageProtein { get; set; }

    /// <summary>
    /// Comparison insights
    /// </summary>
    public List<string> Insights { get; set; } = new();
}
