using System.ComponentModel.DataAnnotations;
using FoodService.DTOs.Common;
using FoodService.DTOs.Food;
using FoodService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodService.Controllers;

[ApiController]
[Route("api/v1/foods")]
public class FoodController(IFoodService foodService, ILogger<FoodController> logger) : ControllerBase
{
    /// <summary>
    /// Get all foods with pagination
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<FoodListDto>>>> GetFoods([FromQuery] PaginationRequest request)
    {
        try
        {
            var result = await foodService.GetFoodsAsync(request);
            return Ok(ApiResponse<PaginatedResponse<FoodListDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Caused exception while getting foods");
            return StatusCode(500, ApiResponse<PaginatedResponse<FoodListDto>>.ErrorResponse("System Error"));
        }
    }

    /// <summary>
    /// Enhance searching food with details (with name, category, calories, v.v...)
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<FoodListDto>>>> SearchFoods([FromQuery] FoodSearchRequest request)
    {
        try
        {
            var result = await foodService.SearchFoodsAsync(request);
            return Ok(ApiResponse<PaginatedResponse<FoodListDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while searching foods");
            return StatusCode(500, ApiResponse<PaginatedResponse<FoodListDto>>.ErrorResponse("System Error"));
        }
    }

    /// <summary>
    /// Get food details with ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<FoodDetailsDto>>> GetFoodById(Guid id)
    {
        try
        {
            var food = await foodService.GetFoodByIdAsync(id);
            if (food == null)
                return NotFound(ApiResponse<FoodDetailsDto>.ErrorResponse("Cannot found food"));
            return Ok(ApiResponse<FoodDetailsDto>.SuccessResponse(food));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting foods details with ID: {Id}", id);
            return StatusCode(500, ApiResponse<FoodDetailsDto>.ErrorResponse("System Error"));
        }
    }

    /// <summary>
    /// Create new food
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<FoodDetailsDto>>> CreateFood([FromBody] CreateFoodDto foodDto)
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
                return BadRequest(ApiResponse<FoodDetailsDto>.ValidationErrorResponse(errors));
            }

            var createdFood = await foodService.CreateFoodAsync(foodDto);
            return CreatedAtAction(nameof(GetFoodById), new { id = createdFood.Id }, ApiResponse<FoodDetailsDto>.SuccessResponse(createdFood, "Created Food Successfully"));
        }
        catch (ValidationException ex)
        {
            logger.LogError(ex, "Error while creating food");
            return BadRequest(ApiResponse<FoodDetailsDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while creating food");
            return StatusCode(500, ApiResponse<FoodDetailsDto>.ErrorResponse("System Error"));
        }
    }

    /// <summary>
    /// Update specific food with ID
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<FoodDetailsDto>>> UpdateFood(Guid id, UpdateFoodDto foodDto)
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
                return BadRequest(ApiResponse<FoodDetailsDto>.ValidationErrorResponse(errors));
            }

            var updatedFood = await foodService.UpdateFoodAsync(id, foodDto);
            
            return Ok(ApiResponse<FoodDetailsDto>.SuccessResponse(updatedFood!, "Updated Food with Successfully"));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<FoodDetailsDto>.ErrorResponse("Not found food"));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ApiResponse<FoodDetailsDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while updating food");
            return StatusCode(500, ApiResponse<FoodDetailsDto>.ErrorResponse("System Error"));
        }
    }
}