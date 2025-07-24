using System.ComponentModel.DataAnnotations;
using FoodService.Data;
using FoodService.DTOs.Common;
using FoodService.DTOs.Nutrition;
using FoodService.RequestHelpers.Nutrition;
using FoodService.ResponseHelpers.Nutrition;
using FoodService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodService.Controllers;

[ApiController]
[Route("api/v1/nutrition")]
public class NutritionController(INutritionCalculationService nutritionCalculation, ILogger<NutritionController> logger,
    INutritionFactsService nutritionFactsService, FoodDbContext context) : ControllerBase
{
    /// <summary>
    /// Get all nutrition facts with pagination
    /// Will implement authentication
    /// </summary>
    /// <param name="request">Request</param>
    /// <returns>All nutrition facts with pagination or null if not found</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<NutritionFactsDto>>>> GetAllNutritionFacts([FromQuery] PaginationRequest request)
    {
        try
        {
            var result = await nutritionFactsService.GetAllNutritionAsync(request);
            return Ok(ApiResponse<PaginatedResponse<NutritionFactsDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting categories");
            return StatusCode(500, ApiResponse<PaginatedResponse<NutritionFactsDto>>.ErrorResponse("System Error"));
        }
    }
    /// <summary>
    /// Get nutrition facts with ID
    /// Will implement authentication
    /// </summary>
    /// <param name="id">ID of Nutrition Fact</param>
    /// <returns>Nutrition fact or null if not found</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<NutritionFactsDto>>> GetNutritionFact(Guid id)
    {
        try
        {
            var nutrition = await nutritionFactsService.GetNutritionByIdAsync(id);
            if (nutrition == null)
                return NotFound(ApiResponse<NutritionFactsDto>.ErrorResponse("Nutrition not found")); 
            return Ok(ApiResponse<NutritionFactsDto>.SuccessResponse(nutrition));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting nutrition details with ID: {Id}", id);
            return StatusCode(500, ApiResponse<NutritionFactsDto>.ErrorResponse("System Error"));
        }
    }
    /// <summary>
    /// Create new nutrition food
    /// Will implement authentication
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CreateNutritionFactsDto>>> CreateNutritionFactAsync(
        CreateNutritionFactsDto nutritionFactsDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value is { Errors.Count: > 0 })
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                    );
                return BadRequest(ApiResponse<NutritionFactsDto>.ValidationErrorResponse(errors));
            }
            var foodExists = await context.Foods.AnyAsync(f => f.Id == nutritionFactsDto.FoodId);
            if (!foodExists)
                throw new ValidationException("FoodId không tồn tại trong hệ thống.");

            var exists = await context.NutritionFacts.AnyAsync(n => n.FoodId == nutritionFactsDto.FoodId);
            if (exists)
                throw new InvalidOperationException("Đã tồn tại NutritionFacts cho món ăn này");
            
            var createNutrition = await nutritionFactsService.CreateNutritionAsync(nutritionFactsDto);
            return CreatedAtAction(nameof(GetNutritionFact),  new { id = createNutrition.Id }, ApiResponse<NutritionFactsDto>.SuccessResponse(createNutrition, "Created Nutrition Fact Successfully"));
        }
        catch (ValidationException ex)
        {
            logger.LogError(ex, "Error while creating Nutrition fact");
            return BadRequest(ApiResponse<NutritionFactsDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while creating Nutrition fact");
            return StatusCode(500, ApiResponse<NutritionFactsDto>.ErrorResponse("System Error"));
        }
    }

    /// <summary>
    /// Update specific nutrition fact with ID
    /// Will implement authentication
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<UpdateNutritionFactsDto>>> UpdateNutritionAsync(Guid id,
        UpdateNutritionFactsDto nutritionFactsDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value is { Errors.Count: > 0 })
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                    );
                
                return BadRequest(ApiResponse<NutritionFactsDto>.ValidationErrorResponse(errors));
            }
            
            var updatedNutrition = await nutritionFactsService.UpdateNutritionAsync(id, nutritionFactsDto);
            if (updatedNutrition == null)
                return NotFound(ApiResponse<NutritionFactsDto>.ErrorResponse("Nutrition not found"));
            return Ok(ApiResponse<NutritionFactsDto>.SuccessResponse(updatedNutrition, "Updated Nutrition Fact Successfully"));
        }
        catch (ValidationException ex)
        {
            logger.LogError(ex, "Error while updating Nutrition fact");
            return BadRequest(ApiResponse<NutritionFactsDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while updating Nutrition fact");
            return StatusCode(500, ApiResponse<NutritionFactsDto>.ErrorResponse("System Error"));
        }
    }
    /// <summary>
    /// Delete existing nutrition fact
    /// Will implement authentication
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteNutrition(Guid id)
    {
        try
        {
            var nutrition = await nutritionFactsService.GetNutritionByIdAsync(id);
            if (nutrition == null)
                return NotFound(ApiResponse<NutritionFactsDto>.ErrorResponse("Nutrition not found"));

            await nutritionFactsService.DeleteNutritionAsync(id);
            return Ok(ApiResponse<NutritionFactsDto>.DeleteResponse("Deleted nutrition"));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while deleting nutrition");
            return StatusCode(500, ApiResponse<NutritionFactsDto>.ErrorResponse("System Error"));
        }
    }
    /// <summary>
    /// Tính dinh dưỡng cho khẩu phần bất kỳ
    /// </summary>
    [HttpGet("calculate")]
    public async Task<ActionResult<ApiResponse<ServingNutritionResponse>>> CalculateNutrition([FromQuery] Guid foodId, [FromQuery] decimal servingGrams)
    {
        if (foodId == Guid.Empty || servingGrams <= 0)
            return BadRequest(ApiResponse<ServingNutritionResponse>.ErrorResponse("foodId và servingGrams phải hợp lệ"));
        try
        {
            var result = await nutritionCalculation.CalculateNutritionForServingAsync(foodId, servingGrams);
            if (result == null)
                return NotFound(ApiResponse<ServingNutritionResponse>.ErrorResponse("Không tìm thấy món ăn hoặc dữ liệu dinh dưỡng"));
            return Ok(ApiResponse<ServingNutritionResponse>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Lỗi khi tính dinh dưỡng cho khẩu phần");
            return StatusCode(500, ApiResponse<ServingNutritionResponse>.ErrorResponse("Lỗi hệ thống"));
        }
    }

    /// <summary>
    /// Tính % daily value cho khẩu phần
    /// </summary>
    [HttpGet("daily-values")]
    public async Task<ActionResult<ApiResponse<List<DailyValuePercentageResponse>>>> GetDailyValues([FromQuery] Guid foodId, [FromQuery] decimal servingGrams)
    {
        if (foodId == Guid.Empty || servingGrams <= 0)
            return BadRequest(ApiResponse<List<DailyValuePercentageResponse>>.ErrorResponse("foodId và servingGrams phải hợp lệ"));
        try
        {
            var serving = await nutritionCalculation.CalculateNutritionForServingAsync(foodId, servingGrams);
            if (serving == null)
                return NotFound(ApiResponse<List<DailyValuePercentageResponse>>.ErrorResponse("Không tìm thấy món ăn hoặc dữ liệu dinh dưỡng"));
            var result = await nutritionCalculation.GetServingDailyValuesAsync(serving);
            return Ok(ApiResponse<List<DailyValuePercentageResponse>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Lỗi khi tính daily value");
            return StatusCode(500, ApiResponse<List<DailyValuePercentageResponse>>.ErrorResponse("Lỗi hệ thống"));
        }
    }

    /// <summary>
    /// So sánh dinh dưỡng nhiều món ăn
    /// </summary>
    [HttpPost("compare")]
    public async Task<ActionResult<ApiResponse<NutritionComparisonResponse>>> CompareFoods([FromBody] List<Guid> foodIds)
    {
        if (foodIds.Count == 0)
            return BadRequest(ApiResponse<NutritionComparisonResponse>.ErrorResponse("Danh sách foodIds không hợp lệ"));
        try
        {
            var result = await nutritionCalculation.GetNutritionComparisonAsync(foodIds);
            return Ok(ApiResponse<NutritionComparisonResponse>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Lỗi khi so sánh dinh dưỡng");
            return StatusCode(500, ApiResponse<NutritionComparisonResponse>.ErrorResponse("Lỗi hệ thống"));
        }
    }

    /// <summary>
    /// Tính dinh dưỡng cho recipe (nhiều nguyên liệu)
    /// </summary>
    [HttpPost("recipe")]
    public async Task<ActionResult<ApiResponse<ServingNutritionResponse>>> CalculateRecipeNutrition([FromBody] List<RecipeIngredientRequest> ingredients)
    {
        if (ingredients.Count == 0)
            return BadRequest(ApiResponse<ServingNutritionResponse>.ErrorResponse("Danh sách nguyên liệu không hợp lệ"));
        try
        {
            var result = await nutritionCalculation.CalculateRecipeNutritionAsync(ingredients);
            return Ok(ApiResponse<ServingNutritionResponse>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Lỗi khi tính dinh dưỡng cho recipe");
            return StatusCode(500, ApiResponse<ServingNutritionResponse>.ErrorResponse("Lỗi hệ thống"));
        }
    }

    /// <summary>
    /// Tính điểm mật độ dinh dưỡng (nutrition density score)
    /// </summary>
    [HttpGet("density/{foodId}")]
    public async Task<ActionResult<ApiResponse<decimal>>> GetNutritionDensityScore([FromRoute] Guid foodId)
    {
        if (foodId == Guid.Empty)
            return BadRequest(ApiResponse<decimal>.ErrorResponse("foodId không hợp lệ"));
        try
        {
            var result = await nutritionCalculation.CalculateNutritionDensityScoreAsync(foodId);
            return Ok(ApiResponse<decimal>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Lỗi khi tính density score");
            return StatusCode(500, ApiResponse<decimal>.ErrorResponse("Lỗi hệ thống"));
        }
    }

    /// <summary>
    /// Lấy danh sách khẩu phần thay thế với dinh dưỡng
    /// </summary>
    [HttpGet("alternatives/{foodId}")]
    public async Task<ActionResult<ApiResponse<List<ServingNutritionResponse>>>> GetAlternativeServingSizes([FromRoute] Guid foodId)
    {
        if (foodId == Guid.Empty)
            return BadRequest(ApiResponse<List<ServingNutritionResponse>>.ErrorResponse("foodId không hợp lệ"));
        try
        {
            var result = await nutritionCalculation.GetAlternativeServingSizesAsync(foodId);
            return Ok(ApiResponse<List<ServingNutritionResponse>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Lỗi khi lấy alternative serving sizes");
            return StatusCode(500, ApiResponse<List<ServingNutritionResponse>>.ErrorResponse("Lỗi hệ thống"));
        }
    }
}