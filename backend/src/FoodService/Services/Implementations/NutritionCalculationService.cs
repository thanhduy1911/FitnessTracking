using Microsoft.EntityFrameworkCore;
using FoodService.Data;
using FoodService.RequestHelpers.Nutrition;
using FoodService.ResponseHelpers.Nutrition;
using FoodService.Services.Interfaces;
using System.Text.Json;

namespace FoodService.Services.Implementations;

/// <summary>
/// Implementation of INutritionCalculationService
/// Provides nutrition calculations, serving size calculations, and nutrition analysis
/// </summary>
public class NutritionCalculationService : INutritionCalculationService
{
    private readonly FoodDbContext _context;

    public NutritionCalculationService(FoodDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Daily value reference values (based on 2000 calorie diet)
    /// </summary>
    private static readonly Dictionary<string, decimal> DailyValues = new()
    {
        // Macronutrients
        ["Protein"] = 50m,           // 50g
        ["Carbs"] = 300m,            // 300g
        ["Fat"] = 65m,               // 65g
        ["Fiber"] = 25m,             // 25g
        ["Sugar"] = 50m,             // 50g (added sugars)
        ["Sodium"] = 2300m,          // 2300mg
        ["Cholesterol"] = 300m,      // 300mg
        ["SaturatedFat"] = 20m,      // 20g
        
        // Vitamins
        ["VitaminA"] = 900m,         // 900mcg
        ["VitaminC"] = 90m,          // 90mg
        ["VitaminD"] = 20m,          // 20mcg
        ["VitaminE"] = 15m,          // 15mg
        ["VitaminK"] = 120m,         // 120mcg
        ["VitaminB1"] = 1.2m,        // 1.2mg (Thiamine)
        ["VitaminB2"] = 1.3m,        // 1.3mg (Riboflavin)
        ["VitaminB3"] = 16m,         // 16mg (Niacin)
        ["VitaminB6"] = 1.7m,        // 1.7mg
        ["VitaminB12"] = 2.4m,       // 2.4mcg
        ["Folate"] = 400m,           // 400mcg
        ["Biotin"] = 30m,            // 30mcg
        ["PantothenicAcid"] = 5m,    // 5mg
        ["Choline"] = 550m,          // 550mg
        
        // Minerals
        ["Calcium"] = 1000m,         // 1000mg
        ["Iron"] = 18m,              // 18mg
        ["Magnesium"] = 400m,        // 400mg
        ["Phosphorus"] = 700m,       // 700mg
        ["Potassium"] = 3500m,       // 3500mg
        ["Zinc"] = 11m,              // 11mg
        ["Copper"] = 0.9m,           // 0.9mg
        ["Manganese"] = 2.3m,        // 2.3mg
        ["Selenium"] = 55m,          // 55mcg
    };

    /// <summary>
    /// Calculate nutrition for a specific serving size
    /// </summary>
    public async Task<ServingNutritionResponse?> CalculateNutritionForServingAsync(Guid foodId, decimal servingGrams)
    {
        var food = await _context.Foods
            .Include(f => f.NutritionFacts)
            .FirstOrDefaultAsync(f => f.Id == foodId && f.IsActive);

        if (food?.NutritionFacts == null)
            return null;

        var nutrition = food.NutritionFacts;
        var multiplier = servingGrams / 100m; // Nutrition facts are per 100g

        return new ServingNutritionResponse
        {
            FoodId = foodId,
            FoodName = food.Name,
            ServingGrams = servingGrams,
            ServingDescription = GetServingDescription(food, servingGrams),
            Multiplier = multiplier,
            
            // Calculate nutrition values
            CaloriesKcal = nutrition.CaloriesKcal * multiplier,
            ProteinG = nutrition.ProteinG * multiplier,
            CarbsG = nutrition.CarbohydrateG * multiplier,
            FatG = nutrition.FatG * multiplier,
            FiberG = nutrition.FiberG * multiplier,
            SugarG = nutrition.SugarG * multiplier,
            SodiumMg = nutrition.SodiumMg * multiplier,
            CalciumMg = nutrition.CalciumMg * multiplier,
            IronMg = nutrition.IronMg * multiplier,
            VitaminCMg = nutrition.VitaminCMg * multiplier,
            VitaminAMcg = nutrition.VitaminAMcg * multiplier,
            SaturatedFatG = nutrition.SaturatedFatG * multiplier,
            CholesterolMg = nutrition.CholesterolMg * multiplier,
            PotassiumMg = nutrition.PotassiumMg * multiplier,
            MagnesiumMg = nutrition.MagnesiumMg * multiplier,
            ZincMg = nutrition.ZincMg * multiplier,
            VitaminB6Mg = nutrition.VitaminB6Mg * multiplier,
            VitaminB12Mcg = nutrition.VitaminB12Mcg * multiplier,
            FolateMcg = nutrition.FolateMcg * multiplier,
            BiotinMcg = nutrition.BiotinMcg * multiplier,
            CholineMg = nutrition.CholineMg * multiplier,
            CopperMg = nutrition.CopperMg * multiplier,
            ManganeseMg = nutrition.ManganeseMg * multiplier,
            SeleniumMcg = nutrition.SeleniumMcg * multiplier,
            PantothenicAcidMg = nutrition.PantothenicAcidMg * multiplier
        };
    }

    /// <summary>
    /// Calculate daily value percentage for a nutrient
    /// </summary>
    public async Task<DailyValuePercentageResponse?> GetDailyValuePercentageAsync(decimal nutrientValue, string nutrientType)
    {
        await Task.CompletedTask; // Async for consistency

        if (!DailyValues.TryGetValue(nutrientType, out var dailyValue))
            return null;

        var percentage = (nutrientValue / dailyValue) * 100m;
        var (category, categoryVi) = GetNutrientCategory(percentage);
        var (unit, nameVi) = GetNutrientInfo(nutrientType);

        return new DailyValuePercentageResponse
        {
            NutrientName = nutrientType,
            NutrientNameVi = nameVi,
            NutrientValue = nutrientValue,
            Unit = unit,
            DailyValueReference = dailyValue,
            Percentage = Math.Round(percentage, 1),
            Category = category,
            CategoryVi = categoryVi,
            HealthRecommendation = GetHealthRecommendation(nutrientType, percentage),
            HealthRecommendationVi = GetHealthRecommendationVi(nutrientType, percentage)
        };
    }

    /// <summary>
    /// Get recommended serving size based on user profile
    /// </summary>
    public async Task<ServingNutritionResponse?> GetRecommendedServingSizeAsync(Guid foodId, string userProfile)
    {
        var food = await _context.Foods
            .Include(f => f.NutritionFacts)
            .Include(f => f.Category)
            .FirstOrDefaultAsync(f => f.Id == foodId && f.IsActive);

        if (food?.NutritionFacts == null)
            return null;

        var recommendedGrams = CalculateRecommendedServingSize(food, userProfile);
        return await CalculateNutritionForServingAsync(foodId, recommendedGrams);
    }

    /// <summary>
    /// Calculate total nutrition for a recipe
    /// </summary>
    public async Task<ServingNutritionResponse> CalculateRecipeNutritionAsync(List<RecipeIngredientRequest> ingredients)
    {
        var totalNutrition = new ServingNutritionResponse
        {
            FoodName = "Recipe Total",
            ServingDescription = "Toàn bộ công thức"
        };

        decimal totalWeight = 0;

        foreach (var ingredient in ingredients)
        {
            var ingredientNutrition = await CalculateNutritionForServingAsync(ingredient.FoodId, ingredient.QuantityGrams);
            if (ingredientNutrition != null)
            {
                totalNutrition.CaloriesKcal += ingredientNutrition.CaloriesKcal ?? 0;
                totalNutrition.ProteinG += ingredientNutrition.ProteinG ?? 0;
                totalNutrition.CarbsG += ingredientNutrition.CarbsG ?? 0;
                totalNutrition.FatG += ingredientNutrition.FatG ?? 0;
                totalNutrition.FiberG += ingredientNutrition.FiberG ?? 0;
                totalNutrition.SugarG += ingredientNutrition.SugarG ?? 0;
                totalNutrition.SodiumMg += ingredientNutrition.SodiumMg ?? 0;
                totalNutrition.CalciumMg += ingredientNutrition.CalciumMg ?? 0;
                totalNutrition.IronMg += ingredientNutrition.IronMg ?? 0;
                totalNutrition.VitaminCMg += ingredientNutrition.VitaminCMg ?? 0;
                totalNutrition.VitaminAMcg += ingredientNutrition.VitaminAMcg ?? 0;
                totalNutrition.SaturatedFatG += ingredientNutrition.SaturatedFatG ?? 0;
                totalNutrition.CholesterolMg += ingredientNutrition.CholesterolMg ?? 0;
                totalNutrition.PotassiumMg += ingredientNutrition.PotassiumMg ?? 0;
                totalNutrition.MagnesiumMg += ingredientNutrition.MagnesiumMg ?? 0;
                totalNutrition.ZincMg += ingredientNutrition.ZincMg ?? 0;
                totalNutrition.VitaminB6Mg += ingredientNutrition.VitaminB6Mg ?? 0;
                totalNutrition.VitaminB12Mcg += ingredientNutrition.VitaminB12Mcg ?? 0;
                totalNutrition.FolateMcg += ingredientNutrition.FolateMcg ?? 0;
                totalNutrition.BiotinMcg += ingredientNutrition.BiotinMcg ?? 0;
                totalNutrition.CholineMg += ingredientNutrition.CholineMg ?? 0;
                totalNutrition.CopperMg += ingredientNutrition.CopperMg ?? 0;
                totalNutrition.ManganeseMg += ingredientNutrition.ManganeseMg ?? 0;
                totalNutrition.SeleniumMcg += ingredientNutrition.SeleniumMcg ?? 0;
                totalNutrition.PantothenicAcidMg += ingredientNutrition.PantothenicAcidMg ?? 0;
            }

            totalWeight += ingredient.QuantityGrams;
        }

        totalNutrition.ServingGrams = totalWeight;
        totalNutrition.Multiplier = totalWeight / 100m;

        return totalNutrition;
    }

    /// <summary>
    /// Compare nutrition between multiple foods
    /// </summary>
    public async Task<NutritionComparisonResponse> GetNutritionComparisonAsync(List<Guid> foodIds)
    {
        var comparison = new NutritionComparisonResponse();

        foreach (var foodId in foodIds)
        {
            var food = await _context.Foods
                .Include(f => f.NutritionFacts)
                .FirstOrDefaultAsync(f => f.Id == foodId && f.IsActive);

            if (food?.NutritionFacts == null)
                continue;

            var servingGrams = food.ServingSizeGrams ?? 100m;
            var nutrition = await CalculateNutritionForServingAsync(foodId, servingGrams);

            if (nutrition != null)
            {
                comparison.Foods.Add(new FoodComparisonItem
                {
                    FoodId = foodId,
                    FoodName = food.Name,
                    FoodNameVi = food.NameVi,
                    ServingGrams = servingGrams,
                    ServingDescription = food.ServingSizeDescription ?? $"{servingGrams}g",
                    CaloriesKcal = nutrition.CaloriesKcal,
                    ProteinG = nutrition.ProteinG,
                    CarbsG = nutrition.CarbsG,
                    FatG = nutrition.FatG,
                    FiberG = nutrition.FiberG,
                    SugarG = nutrition.SugarG,
                    SodiumMg = nutrition.SodiumMg,
                    CalciumMg = nutrition.CalciumMg,
                    IronMg = nutrition.IronMg,
                    VitaminCMg = nutrition.VitaminCMg,
                    VitaminAMcg = nutrition.VitaminAMcg
                });
            }
        }

        // Generate comparison summary
        comparison.Summary = GenerateComparisonSummary(comparison.Foods);

        return comparison;
    }

    /// <summary>
    /// Get multiple daily value percentages for a serving
    /// </summary>
    public async Task<List<DailyValuePercentageResponse>> GetServingDailyValuesAsync(ServingNutritionResponse servingNutrition)
    {
        var dailyValues = new List<DailyValuePercentageResponse>();

        // Add all nutrients with values
        if (servingNutrition.ProteinG.HasValue)
            dailyValues.Add(await GetDailyValuePercentageAsync(servingNutrition.ProteinG.Value, "Protein") ?? new DailyValuePercentageResponse());

        if (servingNutrition.CarbsG.HasValue)
            dailyValues.Add(await GetDailyValuePercentageAsync(servingNutrition.CarbsG.Value, "Carbs") ?? new DailyValuePercentageResponse());

        if (servingNutrition.FatG.HasValue)
            dailyValues.Add(await GetDailyValuePercentageAsync(servingNutrition.FatG.Value, "Fat") ?? new DailyValuePercentageResponse());

        if (servingNutrition.FiberG.HasValue)
            dailyValues.Add(await GetDailyValuePercentageAsync(servingNutrition.FiberG.Value, "Fiber") ?? new DailyValuePercentageResponse());

        if (servingNutrition.SodiumMg.HasValue)
            dailyValues.Add(await GetDailyValuePercentageAsync(servingNutrition.SodiumMg.Value, "Sodium") ?? new DailyValuePercentageResponse());

        if (servingNutrition.CalciumMg.HasValue)
            dailyValues.Add(await GetDailyValuePercentageAsync(servingNutrition.CalciumMg.Value, "Calcium") ?? new DailyValuePercentageResponse());

        if (servingNutrition.IronMg.HasValue)
            dailyValues.Add(await GetDailyValuePercentageAsync(servingNutrition.IronMg.Value, "Iron") ?? new DailyValuePercentageResponse());

        if (servingNutrition.VitaminCMg.HasValue)
            dailyValues.Add(await GetDailyValuePercentageAsync(servingNutrition.VitaminCMg.Value, "VitaminC") ?? new DailyValuePercentageResponse());

        if (servingNutrition.VitaminAMcg.HasValue)
            dailyValues.Add(await GetDailyValuePercentageAsync(servingNutrition.VitaminAMcg.Value, "VitaminA") ?? new DailyValuePercentageResponse());

        return dailyValues.Where(dv => !string.IsNullOrEmpty(dv.NutrientName)).ToList();
    }

    /// <summary>
    /// Calculate nutrition density score (nutrition per calorie)
    /// </summary>
    public async Task<decimal> CalculateNutritionDensityScoreAsync(Guid foodId)
    {
        var nutrition = await CalculateNutritionForServingAsync(foodId, 100m);
        if (nutrition?.CaloriesKcal == null || nutrition.CaloriesKcal <= 0)
            return 0;

        var score = 0m;
        var calories = nutrition.CaloriesKcal.Value;

        // Calculate score based on nutrient density
        if (nutrition.ProteinG.HasValue) score += (nutrition.ProteinG.Value / calories) * 100;
        if (nutrition.FiberG.HasValue) score += (nutrition.FiberG.Value / calories) * 100;
        if (nutrition.VitaminCMg.HasValue) score += (nutrition.VitaminCMg.Value / calories) * 10;
        if (nutrition.VitaminAMcg.HasValue) score += (nutrition.VitaminAMcg.Value / calories) * 1;
        if (nutrition.CalciumMg.HasValue) score += (nutrition.CalciumMg.Value / calories) * 10;
        if (nutrition.IronMg.HasValue) score += (nutrition.IronMg.Value / calories) * 100;

        // Penalize for high sodium and saturated fat
        if (nutrition.SodiumMg.HasValue) score -= (nutrition.SodiumMg.Value / calories) * 1;
        if (nutrition.SaturatedFatG.HasValue) score -= (nutrition.SaturatedFatG.Value / calories) * 50;

        return Math.Max(0, Math.Min(100, score));
    }

    /// <summary>
    /// Get alternative serving sizes with nutrition for a food
    /// </summary>
    public async Task<List<ServingNutritionResponse>> GetAlternativeServingSizesAsync(Guid foodId)
    {
        var food = await _context.Foods
            .Include(f => f.NutritionFacts)
            .FirstOrDefaultAsync(f => f.Id == foodId && f.IsActive);

        if (food?.NutritionFacts == null)
            return new List<ServingNutritionResponse>();

        var servingSizes = new List<ServingNutritionResponse>();

        // Always include 100g
        var base100g = await CalculateNutritionForServingAsync(foodId, 100m);
        if (base100g != null)
        {
            base100g.ServingDescription = "100g";
            servingSizes.Add(base100g);
        }

        // Add default serving size
        if (food.ServingSizeGrams.HasValue)
        {
            var defaultServing = await CalculateNutritionForServingAsync(foodId, food.ServingSizeGrams.Value);
            if (defaultServing != null)
            {
                defaultServing.ServingDescription = food.ServingSizeDescription ?? $"{food.ServingSizeGrams}g";
                servingSizes.Add(defaultServing);
            }
        }

        // Add alternative serving sizes from JSON
        if (!string.IsNullOrEmpty(food.AlternativeServingSizes))
        {
            try
            {
                var alternativeServings = JsonSerializer.Deserialize<List<AlternativeServingDto>>(food.AlternativeServingSizes);
                if (alternativeServings != null)
                {
                    foreach (var alt in alternativeServings)
                    {
                        var altNutrition = await CalculateNutritionForServingAsync(foodId, alt.Grams);
                        if (altNutrition != null)
                        {
                            altNutrition.ServingDescription = alt.Description;
                            servingSizes.Add(altNutrition);
                        }
                    }
                }
            }
            catch (JsonException)
            {
                // Handle JSON parsing errors gracefully
            }
        }

        return servingSizes;
    }

    // Helper methods

    private string GetServingDescription(Entities.Food food, decimal servingGrams)
    {
        if (food.ServingSizeGrams.HasValue && Math.Abs(servingGrams - food.ServingSizeGrams.Value) < 0.1m)
            return food.ServingSizeDescription ?? $"{servingGrams}g";
        
        return $"{servingGrams}g";
    }

    private (string category, string categoryVi) GetNutrientCategory(decimal percentage)
    {
        return percentage switch
        {
            < 5 => ("low", "thấp"),
            < 15 => ("medium", "trung bình"),
            < 25 => ("high", "cao"),
            _ => ("very_high", "rất cao")
        };
    }

    private (string unit, string nameVi) GetNutrientInfo(string nutrientType)
    {
        return nutrientType switch
        {
            "Protein" => ("g", "Protein"),
            "Carbs" => ("g", "Carbohydrate"),
            "Fat" => ("g", "Chất béo"),
            "Fiber" => ("g", "Chất xơ"),
            "Sugar" => ("g", "Đường"),
            "Sodium" => ("mg", "Natri"),
            "Calcium" => ("mg", "Canxi"),
            "Iron" => ("mg", "Sắt"),
            "VitaminC" => ("mg", "Vitamin C"),
            "VitaminA" => ("mcg", "Vitamin A"),
            "VitaminD" => ("mcg", "Vitamin D"),
            "VitaminE" => ("mg", "Vitamin E"),
            "VitaminK" => ("mcg", "Vitamin K"),
            "Potassium" => ("mg", "Kali"),
            "Magnesium" => ("mg", "Magiê"),
            "Zinc" => ("mg", "Kẽm"),
            "Copper" => ("mg", "Đồng"),
            "Manganese" => ("mg", "Mangan"),
            "Selenium" => ("mcg", "Selen"),
            "Biotin" => ("mcg", "Biotin"),
            "PantothenicAcid" => ("mg", "Acid Pantothenic"),
            "Choline" => ("mg", "Choline"),
            _ => ("", nutrientType)
        };
    }

    private string? GetHealthRecommendation(string nutrientType, decimal percentage)
    {
        return (nutrientType, percentage) switch
        {
            ("Sodium", > 20) => "High sodium content. Consider limiting intake.",
            ("SaturatedFat", > 15) => "High saturated fat. Consider moderation.",
            ("Fiber", > 20) => "Good source of fiber!",
            ("Protein", > 20) => "High protein content!",
            ("VitaminC", > 20) => "Good source of Vitamin C!",
            ("Calcium", > 20) => "Good source of calcium!",
            ("Iron", > 20) => "Good source of iron!",
            _ => null
        };
    }

    private string? GetHealthRecommendationVi(string nutrientType, decimal percentage)
    {
        return (nutrientType, percentage) switch
        {
            ("Sodium", > 20) => "Hàm lượng natri cao. Nên hạn chế sử dụng.",
            ("SaturatedFat", > 15) => "Chất béo bão hòa cao. Nên ăn vừa phải.",
            ("Fiber", > 20) => "Nguồn chất xơ tốt!",
            ("Protein", > 20) => "Hàm lượng protein cao!",
            ("VitaminC", > 20) => "Nguồn Vitamin C tốt!",
            ("Calcium", > 20) => "Nguồn canxi tốt!",
            ("Iron", > 20) => "Nguồn sắt tốt!",
            _ => null
        };
    }

    private decimal CalculateRecommendedServingSize(Entities.Food food, string userProfile)
    {
        var baseServing = food.ServingSizeGrams ?? 100m;
        var categoryName = food.Category?.Name?.ToLower() ?? "";

        return userProfile.ToLower() switch
        {
            "weight_loss" => baseServing * 0.8m,
            "muscle_gain" => baseServing * 1.2m,
            "maintenance" => baseServing,
            "athlete" => baseServing * 1.5m,
            "elderly" => baseServing * 0.9m,
            _ => baseServing
        };
    }

    private ComparisonSummary GenerateComparisonSummary(List<FoodComparisonItem> foods)
    {
        if (!foods.Any())
            return new ComparisonSummary();

        var summary = new ComparisonSummary();

        // Find extremes
        var highestCaloriesFood = foods.OrderByDescending(f => f.CaloriesKcal ?? 0).First();
        var lowestCaloriesFood = foods.OrderBy(f => f.CaloriesKcal ?? 0).First();
        var highestProteinFood = foods.OrderByDescending(f => f.ProteinG ?? 0).First();
        var lowestFatFood = foods.OrderBy(f => f.FatG ?? 0).First();

        summary.HighestCaloriesFood = highestCaloriesFood.FoodNameVi;
        summary.LowestCaloriesFood = lowestCaloriesFood.FoodNameVi;
        summary.HighestProteinFood = highestProteinFood.FoodNameVi;
        summary.LowestFatFood = lowestFatFood.FoodNameVi;

        // Calculate averages
        summary.AverageCalories = foods.Average(f => f.CaloriesKcal ?? 0);
        summary.AverageProtein = foods.Average(f => f.ProteinG ?? 0);

        // Generate insights
        summary.Insights = GenerateInsights(foods);

        return summary;
    }

    private List<string> GenerateInsights(List<FoodComparisonItem> foods)
    {
        var insights = new List<string>();

        if (foods.Count >= 2)
        {
            var avgCalories = foods.Average(f => f.CaloriesKcal ?? 0);
            var highCalorieFoods = foods.Where(f => (f.CaloriesKcal ?? 0) > avgCalories * 1.5m).ToList();
            var lowCalorieFoods = foods.Where(f => (f.CaloriesKcal ?? 0) < avgCalories * 0.5m).ToList();

            if (highCalorieFoods.Any())
            {
                insights.Add($"Thực phẩm có calories cao nhất: {string.Join(", ", highCalorieFoods.Select(f => f.FoodNameVi))}");
            }

            if (lowCalorieFoods.Any())
            {
                insights.Add($"Thực phẩm có calories thấp nhất: {string.Join(", ", lowCalorieFoods.Select(f => f.FoodNameVi))}");
            }

            var highProteinFoods = foods.Where(f => (f.ProteinG ?? 0) > 10).ToList();
            if (highProteinFoods.Any())
            {
                insights.Add($"Nguồn protein tốt: {string.Join(", ", highProteinFoods.Select(f => f.FoodNameVi))}");
            }
        }

        return insights;
    }

    // Helper class for JSON deserization
    private class AlternativeServingDto
    {
        public decimal Grams { get; set; }
        public string Description { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
    }
} 