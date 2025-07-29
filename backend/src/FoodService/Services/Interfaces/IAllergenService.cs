using FoodService.DTOs.Allergen;
using FoodService.DTOs.Common;

namespace FoodService.Services.Interfaces;

/// <summary>
/// Interface for Allergen service operations
/// Provides CRUD operations and business logic for Allergen entities
/// </summary>
public interface IAllergenService
{
    /// <summary>
    /// Get paginated list of allergens
    /// </summary>
    /// <param name="request">Pagination parameters</param>
    /// <returns>Paginated list of allergens</returns>
    Task<PaginatedResponse<AllergenListDto>> GetAllergensAsync(PaginationRequest request);
    
    /// <summary>
    /// Get food allergen by ID with complete details
    /// </summary>
    /// <param name="id">Allergen ID</param>
    /// <returns>Complete allergen details or null if not found</returns>
    Task<AllergenDto?> GetAllergenByIdAsync(Guid id);
    
    /// <summary>
    /// Create new allergen item
    /// </summary>
    /// <param name="createAllergenDto">Allergen creation data</param>
    /// <returns>Created allergen details</returns>
    Task<AllergenDto> CreateAllergenAsync(CreateAllergenDto createAllergenDto);
    
    /// <summary>
    /// Update existing allergen item
    /// </summary>
    /// <param name="id">Allergen ID</param>
    /// <param name="updateAllergenDto">Allergen update data</param>
    /// <returns>Updated allergen details or null if not found</returns>
    Task<AllergenDto?> UpdateAllergenAsync(Guid id, UpdateAllergenDto updateAllergenDto);
    
    /// <summary>
    /// Delete allergen item (soft delete)
    /// </summary>
    /// <param name="id">Allergen ID</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteAllergenAsync(Guid id);
    
    /// <summary>
    /// Get allergens by Food Id
    /// </summary>
    /// <param name="foodId">Category ID</param>
    /// <returns>List of Allergens with Food ID</returns>
    Task<FoodAllergenDto?> GetAllergenWithFoodByFoodIdAsync(Guid foodId);
}