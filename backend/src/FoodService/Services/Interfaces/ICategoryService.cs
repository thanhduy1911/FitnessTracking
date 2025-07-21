using FoodService.DTOs.Category;
using FoodService.DTOs.Common;

namespace FoodService.Services.Interfaces;

/// <summary>
/// Interface for Category service operations
/// Provides CRUD operations and business logic for FoodCategory entities
/// </summary>
public interface ICategoryService
{
    Task<PaginatedResponse<CategoryListDto>> GetCategoriesAsync(PaginationRequest request);
    Task<CategoryDto?> GetCategoryByIdAsync(Guid id);
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
    Task<CategoryDto?> UpdateCategoryAsync(Guid id, UpdateCategoryDto dto);
    Task<bool> DeleteCategoryAsync(Guid id);
}