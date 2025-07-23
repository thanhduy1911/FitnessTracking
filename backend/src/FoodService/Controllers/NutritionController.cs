using FoodService.DTOs.Common;
using FoodService.RequestHelpers.Nutrition;
using FoodService.ResponseHelpers.Nutrition;
using FoodService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodService.Controllers;

[ApiController]
[Route("api/v1/nutrition")]
public class NutritionController(INutritionCalculationService nutrition, ILogger<NutritionController> logger)
    : ControllerBase
{
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
            var result = await nutrition.CalculateNutritionForServingAsync(foodId, servingGrams);
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
            var serving = await nutrition.CalculateNutritionForServingAsync(foodId, servingGrams);
            if (serving == null)
                return NotFound(ApiResponse<List<DailyValuePercentageResponse>>.ErrorResponse("Không tìm thấy món ăn hoặc dữ liệu dinh dưỡng"));
            var result = await nutrition.GetServingDailyValuesAsync(serving);
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
            var result = await nutrition.GetNutritionComparisonAsync(foodIds);
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
            var result = await nutrition.CalculateRecipeNutritionAsync(ingredients);
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
            var result = await nutrition.CalculateNutritionDensityScoreAsync(foodId);
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
            var result = await nutrition.GetAlternativeServingSizesAsync(foodId);
            return Ok(ApiResponse<List<ServingNutritionResponse>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Lỗi khi lấy alternative serving sizes");
            return StatusCode(500, ApiResponse<List<ServingNutritionResponse>>.ErrorResponse("Lỗi hệ thống"));
        }
    }
}