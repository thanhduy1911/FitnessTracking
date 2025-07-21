using FoodService.DTOs.Category;
using FoodService.DTOs.Common;
using FoodService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodService.Controllers;

[ApiController]
[Route("api/v1/food-categories")]
public class FoodCategoryController(ICategoryService categoryService, ILogger<FoodCategoryController> logger) : ControllerBase
{
    /// <summary>
    /// Get all categories with pagination
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<CategoryListDto>>>> GetCategories([FromQuery] PaginationRequest request)
    {
        try
        {
            var result = await categoryService.GetCategoriesAsync(request);
            return Ok(ApiResponse<PaginatedResponse<CategoryListDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting categories");
            return StatusCode(500, ApiResponse<PaginatedResponse<CategoryListDto>>.ErrorResponse("System Error"));
        }
    }

    /// <summary>
    /// Get category details by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> GetCategoryById(Guid id)
    {
        try
        {
            var category = await categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound(ApiResponse<CategoryDto>.ErrorResponse("Cannot found category"));
            return Ok(ApiResponse<CategoryDto>.SuccessResponse(category));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting category details with ID: {Id}", id);
            return StatusCode(500, ApiResponse<CategoryDto>.ErrorResponse("System Error"));
        }
    }

    /// <summary>
    /// Create new category
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> CreateCategory([FromBody] CreateCategoryDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value is { Errors.Count: > 0 })
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToList()
                    );
                return BadRequest(ApiResponse<CategoryDto>.ValidationErrorResponse(errors));
            }

            var createdCategory = await categoryService.CreateCategoryAsync(dto);
            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, ApiResponse<CategoryDto>.SuccessResponse(createdCategory, "Created Category Successfully"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while creating category");
            return StatusCode(500, ApiResponse<CategoryDto>.ErrorResponse("System Error"));
        }
    }

    /// <summary>
    /// Update category by ID
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> UpdateCategory(Guid id, [FromBody] UpdateCategoryDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value is { Errors.Count: > 0 })
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToList()
                    );
                return BadRequest(ApiResponse<CategoryDto>.ValidationErrorResponse(errors));
            }

            var updatedCategory = await categoryService.UpdateCategoryAsync(id, dto);
            if (updatedCategory == null)
                return NotFound(ApiResponse<CategoryDto>.ErrorResponse("Cannot found category"));
            return Ok(ApiResponse<CategoryDto>.SuccessResponse(updatedCategory, "Updated Category Successfully"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while updating category with ID: {Id}", id);
            return StatusCode(500, ApiResponse<CategoryDto>.ErrorResponse("System Error"));
        }
    }

    /// <summary>
    /// Delete category by ID
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteCategory(Guid id)
    {
        try
        {
            var deleted = await categoryService.DeleteCategoryAsync(id);
            if (!deleted)
                return NotFound(ApiResponse<object>.ErrorResponse("Cannot found category"));
            return Ok(ApiResponse<object>.SuccessResponse( "Deleted Category Successfully"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while deleting category with ID: {Id}", id);
            return StatusCode(500, ApiResponse<object>.ErrorResponse("System Error"));
        }
    }
}