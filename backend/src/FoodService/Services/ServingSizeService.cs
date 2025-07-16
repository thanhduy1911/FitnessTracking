using FoodService.Entities;
using System.Text.Json;

namespace FoodService.Services;

public class ServingSizeService
{
    private ServingSizeCalculation CalculateNutritionPerServing(Food food, decimal servingGrams)
    {
        if (food.NutritionFacts == null)
            throw new ArgumentException("Food does not have nutrition facts");
        
        var multiplier = servingGrams / 100; // Since nutrition facts are per 100g
        
        var nutritionPerServing = new NutritionFacts
        {
            CaloriesKcal = food.NutritionFacts.CaloriesKcal * multiplier,
            ProteinG = food.NutritionFacts.ProteinG * multiplier,
            FatG = food.NutritionFacts.FatG * multiplier,
            CarbohydrateG = food.NutritionFacts.CarbohydrateG * multiplier,
            FiberG = food.NutritionFacts.FiberG * multiplier,
            SugarG = food.NutritionFacts.SugarG * multiplier,
            SodiumMg = food.NutritionFacts.SodiumMg * multiplier,
            PotassiumMg = food.NutritionFacts.PotassiumMg * multiplier,
            CalciumMg = food.NutritionFacts.CalciumMg * multiplier,
            IronMg = food.NutritionFacts.IronMg * multiplier,
            MagnesiumMg = food.NutritionFacts.MagnesiumMg * multiplier,
            PhosphorusMg = food.NutritionFacts.PhosphorusMg * multiplier,
            ZincMg = food.NutritionFacts.ZincMg * multiplier,
            VitaminAMcg = food.NutritionFacts.VitaminAMcg * multiplier,
            VitaminB6Mg = food.NutritionFacts.VitaminB6Mg * multiplier,
            VitaminB12Mcg = food.NutritionFacts.VitaminB12Mcg * multiplier,
            VitaminCMg = food.NutritionFacts.VitaminCMg * multiplier,
            VitaminDMcg = food.NutritionFacts.VitaminDMcg * multiplier,
            VitaminEMg = food.NutritionFacts.VitaminEMg * multiplier,
            VitaminKMcg = food.NutritionFacts.VitaminKMcg * multiplier,
            FolateMcg = food.NutritionFacts.FolateMcg * multiplier,
            SaturatedFatG = food.NutritionFacts.SaturatedFatG * multiplier,
            MonounsaturatedFatG = food.NutritionFacts.MonounsaturatedFatG * multiplier,
            PolyunsaturatedFatG = food.NutritionFacts.PolyunsaturatedFatG * multiplier,
            CholesterolMg = food.NutritionFacts.CholesterolMg * multiplier,
            TransFatG = food.NutritionFacts.TransFatG * multiplier
        };
        
        return new ServingSizeCalculation
        {
            ServingGrams = servingGrams,
            ServingDescription = GetServingDescription(food, servingGrams),
            NutritionPerServing = nutritionPerServing,
            MultiplierFrom100g = multiplier
        };
    }
    
    public List<ServingSizeCalculation> GetAvailableServingSizes(Food food)
    {
        var servingSizes = new List<ServingSizeCalculation> {
            // Always include 100g as base
            CalculateNutritionPerServing(food, 100) };

        // Add default serving size if available
        if (food.ServingSizeGrams.HasValue)
        {
            servingSizes.Add(CalculateNutritionPerServing(food, food.ServingSizeGrams.Value));
        }
        
        // Add alternative serving sizes if available
        if (!string.IsNullOrEmpty(food.AlternativeServingSizes))
        {
            try
            {
                var alternativeServings = JsonSerializer.Deserialize<List<ServingSize>>(food.AlternativeServingSizes);
                if (alternativeServings != null)
                {
                    foreach (var serving in alternativeServings)
                    {
                        var calculation = CalculateNutritionPerServing(food, serving.Grams);
                        calculation.ServingDescription = serving.Description;
                        servingSizes.Add(calculation);
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
    
    public ServingSize GetStandardServingSize(Food food)
    {
        // Try to determine standard serving size based on food category
        var categoryName = food.Category?.Name?.ToLower() ?? "";
        var foodName = food.Name?.ToLower() ?? "";
        
        var servingSize = new ServingSize
        {
            Grams = food.ServingSizeGrams ?? 100,
            Description = food.ServingSizeDescription ?? "100g",
            DescriptionEn = food.ServingSizeDescriptionEn ?? "100g",
            IsDefault = true
        };
        
        // Auto-determine serving size based on food type
        if (categoryName.Contains("cơm") || categoryName.Contains("rice"))
        {
            servingSize.Grams = 150;
            servingSize.Description = "1 bát";
            servingSize.DescriptionEn = "1 bowl";
        }
        else if (categoryName.Contains("phở") || categoryName.Contains("noodle"))
        {
            servingSize.Grams = 350;
            servingSize.Description = "1 tô";
            servingSize.DescriptionEn = "1 bowl";
        }
        else if (categoryName.Contains("fruit") || categoryName.Contains("trái cây"))
        {
            servingSize.Grams = DeterminefruitServingSize(foodName);
            servingSize.Description = "1 quả";
            servingSize.DescriptionEn = "1 piece";
        }
        else if (categoryName.Contains("vegetable") || categoryName.Contains("rau"))
        {
            servingSize.Grams = 100;
            servingSize.Description = "1 dĩa";
            servingSize.DescriptionEn = "1 plate";
        }
        else if (categoryName.Contains("beverage") || categoryName.Contains("đồ uống"))
        {
            servingSize.Grams = 250;
            servingSize.Description = "1 ly";
            servingSize.DescriptionEn = "1 glass";
        }
        
        return servingSize;
    }
    
    private string GetServingDescription(Food food, decimal servingGrams)
    {
        if (servingGrams == 100)
            return "100g";
        
        if (food.ServingSizeGrams.HasValue && Math.Abs(servingGrams - food.ServingSizeGrams.Value) < 0.1m)
            return food.ServingSizeDescription ?? $"{servingGrams}g";
        
        return $"{servingGrams}g";
    }
    
    private decimal DeterminefruitServingSize(string foodName)
    {
        var fruitSizes = new Dictionary<string, decimal>
        {
            ["chuối"] = 120,
            ["cam"] = 150,
            ["táo"] = 180,
            ["xoài"] = 200,
            ["đu đủ"] = 250,
            ["dứa"] = 300,
            ["nho"] = 80,
            ["lê"] = 160,
            ["chôm chôm"] = 20,
            ["nhãn"] = 15,
            ["vải"] = 20,
            ["thanh long"] = 200
        };
        
        foreach (var fruit in fruitSizes)
        {
            if (foodName.Contains(fruit.Key))
                return fruit.Value;
        }
        
        return 150; // Default fruit serving size
    }
} 