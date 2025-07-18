using FoodService.RequestHelpers.Nutrition;
using FoodService.ResponseHelpers.Nutrition;

namespace FoodService.Services.Interfaces;

/// <summary>
/// Interface for nutrition calculation services
/// Provides nutrition calculations, serving size calculations, and nutrition analysis
/// </summary>
public interface INutritionCalculationService
{
    /// <summary>
    /// Calculate nutrition for a specific serving size
    /// </summary>
    /// <param name="foodId">Food ID</param>
    /// <param name="servingGrams">Serving size in grams</param>
    /// <returns>Nutrition facts for the serving size</returns>
    Task<ServingNutritionResponse?> CalculateNutritionForServingAsync(Guid foodId, decimal servingGrams);

    /// <summary>
    /// Calculate daily value percentage for a nutrient
    /// </summary>
    /// <param name="nutrientValue">Nutrient value</param>
    /// <param name="nutrientType">Type of nutrient (e.g., "VitaminC", "Protein", "Sodium")</param>
    /// <returns>Daily value percentage information</returns>
    Task<DailyValuePercentageResponse?> GetDailyValuePercentageAsync(decimal nutrientValue, string nutrientType);

    /// <summary>
    /// Get recommended serving size based on user profile
    /// </summary>
    /// <param name="foodId">Food ID</param>
    /// <param name="userProfile">User profile (e.g., "weight_loss", "muscle_gain", "maintenance")</param>
    /// <returns>Recommended serving size with nutrition info</returns>
    Task<ServingNutritionResponse?> GetRecommendedServingSizeAsync(Guid foodId, string userProfile);

    /// <summary>
    /// Calculate total nutrition for a recipe
    /// </summary>
    /// <param name="ingredients">List of recipe ingredients with quantities</param>
    /// <returns>Total nutrition for the recipe</returns>
    Task<ServingNutritionResponse> CalculateRecipeNutritionAsync(List<RecipeIngredientRequest> ingredients);

    /// <summary>
    /// Compare nutrition between multiple foods
    /// </summary>
    /// <param name="foodIds">List of food IDs to compare</param>
    /// <returns>Nutrition comparison data</returns>
    Task<NutritionComparisonResponse> GetNutritionComparisonAsync(List<Guid> foodIds);

    /// <summary>
    /// Get multiple daily value percentages for a serving
    /// </summary>
    /// <param name="servingNutrition">Serving nutrition data</param>
    /// <returns>List of daily value percentages</returns>
    Task<List<DailyValuePercentageResponse>> GetServingDailyValuesAsync(ServingNutritionResponse servingNutrition);

    /// <summary>
    /// Calculate nutrition density score (nutrition per calorie)
    /// </summary>
    /// <param name="foodId">Food ID</param>
    /// <returns>Nutrition density score (0-100)</returns>
    Task<decimal> CalculateNutritionDensityScoreAsync(Guid foodId);

    /// <summary>
    /// Get alternative serving sizes with nutrition for a food
    /// </summary>
    /// <param name="foodId">Food ID</param>
    /// <returns>List of alternative serving sizes with nutrition</returns>
    Task<List<ServingNutritionResponse>> GetAlternativeServingSizesAsync(Guid foodId);
} 