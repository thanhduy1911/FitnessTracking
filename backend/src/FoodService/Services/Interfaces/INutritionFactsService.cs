using FoodService.DTOs.Common;
using FoodService.DTOs.Nutrition;

namespace FoodService.Services.Interfaces;

/// <summary>
/// Interface for nutrition services
/// Provides nutrition CRUD for administrator
/// </summary>
public interface INutritionFactsService
{
    /// <summary>
    /// Get paginated list of nutrition
    /// </summary>
    /// <param name="request">Pagination parameters</param>
    /// <returns>Paginated list of nutrition</returns>
    Task<PaginatedResponse<NutritionFactsDto>> GetAllNutritionAsync(PaginationRequest request);
    /// <summary>
    /// Get nutrition by ID with complete details
    /// </summary>
    /// <param name="id">Nutrition ID</param>
    /// <returns>Complete nutrition details or null if not found</returns>
    Task<NutritionFactsDto?> GetNutritionByIdAsync(Guid id);
    /// <summary>
    /// Create new nutrition item
    /// </summary>
    /// <param name="nutritionFactsDto">Nutrition creation data</param>
    /// <returns>Created nutrition details</returns>
    Task<NutritionFactsDto> CreateNutritionAsync(CreateNutritionFactsDto nutritionFactsDto);
    /// <summary>
    /// Update existing nutrition item
    /// </summary>
    /// <param name="id">Nutrition ID</param>
    /// <param name="updateNutritionFactsDto">Nutrition update data</param>
    /// <returns>Updated nutrition details or null if not found</returns>
    Task<NutritionFactsDto?> UpdateNutritionAsync(Guid id,UpdateNutritionFactsDto updateNutritionFactsDto);
    /// <summary>
    /// Delete nutrition item (hard delete)
    /// </summary>
    /// <param name="id">Nutrition ID</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteNutritionAsync(Guid id);
}