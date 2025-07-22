using FoodService.DTOs.Nutrition;

namespace FoodService.DTOs.Common;

public class ServingSizeCalculationDto
{
    public decimal ServingGrams { get; set; }
    public string ServingDescription { get; set; } = string.Empty;
    public decimal MultiplierFrom100G { get; set; }
    public NutritionFactsDto NutritionPerServing { get; set; } = new();
}