using System.ComponentModel.DataAnnotations;
using FoodService.DTOs.Allergen;
using FoodService.DTOs.Common;
using FoodService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodService.Controllers;

[ApiController]
[Route("api/v1/allergens")]
public class AllergenController(IAllergenService allergenService, ILogger<AllergenController> logger)
    : ControllerBase
{
    /// <summary>
    /// Get all allergens with pagination
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<AllergenListDto>>>> GetAllergensAsync([FromQuery]PaginationRequest paginationRequest)
    {
        try
        {
            var result = await allergenService.GetAllergensAsync(paginationRequest);
            return Ok(ApiResponse<PaginatedResponse<AllergenListDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Caused exception while getting Allergens");
            return StatusCode(500, ApiResponse<PaginatedResponse<AllergenListDto>>.ErrorResponse("System Error"));
        }
    }

    /// <summary>
    /// Get allergen details with ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<AllergenDto>>> GetAllergenById([FromRoute] Guid id)
    {
        try
        {
            var allergen = await allergenService.GetAllergenByIdAsync(id);
            if (allergen == null)
                return NotFound(ApiResponse<AllergenDto>.ErrorResponse("Allergen not found"));
            return Ok(ApiResponse<AllergenDto>.SuccessResponse(allergen));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Caused exception while getting Allergen");
            return StatusCode(500, ApiResponse<AllergenDto>.ErrorResponse("System Error"));
        }
    }

    /// <summary>
    /// Create new allergen
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<AllergenDto>>> CreateAllergenAsync(
        [FromBody] CreateAllergenDto createAllergenDto)
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
                return BadRequest(ApiResponse<AllergenDto>.ValidationErrorResponse(errors));
            }
            
            var createdAllergen = await allergenService.CreateAllergenAsync(createAllergenDto);
            return CreatedAtAction(nameof(GetAllergenById), new { id = createdAllergen.Id },
                ApiResponse<AllergenDto>.SuccessResponse(createdAllergen, "Created Allergen Successfully"));
        }
        catch (ValidationException ex)
        {
            logger.LogError(ex, "Caused exception while creating Allergen");
            return BadRequest(ApiResponse<AllergenDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Caused exception while creating Allergen");
            return StatusCode(500, ApiResponse<AllergenDto>.ErrorResponse("System Error"));
        }
    }

    /// <summary>
    /// Update allergen
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<AllergenDto>>> UpdateAllergen(Guid id,
        [FromBody] UpdateAllergenDto updateAllergenDto)
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
                return BadRequest(ApiResponse<AllergenDto>.ValidationErrorResponse(errors));
            }
            
            var updatedAllergen = await allergenService.UpdateAllergenAsync(id, updateAllergenDto);
            if (updatedAllergen == null)
                return NotFound(ApiResponse<AllergenDto>.ErrorResponse("Allergen not found"));
                
            return Ok(ApiResponse<AllergenDto>.SuccessResponse(updatedAllergen, "Updated Allergen Successfully"));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ApiResponse<AllergenDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Caused exception while updating Allergen");
            return StatusCode(500, ApiResponse<AllergenDto>.ErrorResponse("System Error"));
        }
    }

    /// <summary>
    /// Delete allergen (soft delete)
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteAllergen(Guid id)
    {
        try
        {
            var isDeleted = await allergenService.DeleteAllergenAsync(id);
            if (!isDeleted)
                return NotFound(ApiResponse<bool>.ErrorResponse("Allergen not found"));
                
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Deleted Allergen Successfully"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Caused exception while deleting Allergen");
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("System Error"));
        }
    }
}