using FoodService.DTOs.Food;
using FoodService.DTOs.Common;

namespace FoodService.Services.Interfaces;

/// <summary>
/// Interface for Food service operations
/// Provides CRUD operations and business logic for Food entities
/// </summary>
public interface IFoodService
{
    /// <summary>
    /// Get food by ID with complete details
    /// </summary>
    /// <param name="id">Food ID</param>
    /// <returns>Complete food details or null if not found</returns>
    Task<FoodDetailsDto?> GetFoodByIdAsync(Guid id);

    /// <summary>
    /// Get paginated list of foods
    /// </summary>
    /// <param name="request">Pagination parameters</param>
    /// <returns>Paginated list of foods</returns>
    Task<PaginatedResponse<FoodListDto>> GetFoodsAsync(PaginationRequest request);

    /// <summary>
    /// Search foods by query and filters
    /// </summary>
    /// <param name="request">Search parameters</param>
    /// <returns>Search results</returns>
    Task<PaginatedResponse<FoodListDto>> SearchFoodsAsync(FoodSearchRequest request);

    /// <summary>
    /// Create new food item
    /// </summary>
    /// <param name="dto">Food creation data</param>
    /// <returns>Created food details</returns>
    Task<FoodDetailsDto> CreateFoodAsync(CreateFoodDto dto);

    /// <summary>
    /// Update existing food item
    /// </summary>
    /// <param name="id">Food ID</param>
    /// <param name="dto">Food update data</param>
    /// <returns>Updated food details or null if not found</returns>
    Task<FoodDetailsDto?> UpdateFoodAsync(Guid id, UpdateFoodDto dto);

    /// <summary>
    /// Delete food item (soft delete)
    /// </summary>
    /// <param name="id">Food ID</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteFoodAsync(Guid id);

    /// <summary>
    /// Verify food item by admin/moderator
    /// </summary>
    /// <param name="id">Food ID</param>
    /// <param name="verifiedBy">ID of user who verified</param>
    /// <returns>True if verified, false if not found</returns>
    Task<bool> VerifyFoodAsync(Guid id, Guid verifiedBy);

    /// <summary>
    /// Get foods by category
    /// </summary>
    /// <param name="categoryId">Category ID</param>
    /// <returns>List of foods in category</returns>
    Task<List<FoodListDto>> GetFoodsByCategoryAsync(Guid categoryId);

    /// <summary>
    /// Get foods by allergen (foods that contain specific allergen)
    /// </summary>
    /// <param name="allergenId">Allergen ID</param>
    /// <returns>List of foods containing allergen</returns>
    Task<List<FoodListDto>> GetFoodsByAllergenAsync(Guid allergenId);
} 