using AutoMapper;
using AutoMapper.QueryableExtensions;
using FoodService.Data;
using FoodService.DTOs.Food;
using FoodService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodService.Controllers;

[ApiController]
[Route("api/v1/foods")]
public class FoodController(FoodDbContext dbContext, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<FoodListDto>>> GetAllFoods(string? date)
    {
        var query = dbContext.Foods.OrderBy(x => x.Name).AsQueryable();
        if (!string.IsNullOrEmpty(date))
        {
            query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }

        return await query.ProjectTo<FoodListDto>(mapper.ConfigurationProvider).ToListAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FoodDetailsDto>> GetFoodById(Guid id)
    {
        var food = await dbContext.Foods
            .Include(x => x.FoodAllergens)
            .Include(x => x.NutritionFacts)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (food == null) return NotFound();
        
        return mapper.Map<FoodDetailsDto>(food);
    }

    [HttpPost]
    public async Task<ActionResult<FoodDetailsDto>> CreateFood([FromBody] CreateFoodDto foodDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .SelectMany(x =>
                    {
                        if (x.Value != null) return x.Value.Errors;
                        throw new InvalidOperationException();
                    })
                    .Select(x => x.ErrorMessage)
                    .ToList();
    
                return BadRequest(new
                {
                    Message = "Error creating food",
                    Errors = errors
                });
            }
            
            // Map DTO to entity
            var food = mapper.Map<Food>(foodDto);
            food.Id = Guid.NewGuid();
            food.CreatedAt = DateTime.UtcNow;
            food.UpdatedAt = DateTime.UtcNow;
            food.IsActive = true;
            food.VerificationStatus = "pending";
            
            dbContext.Foods.Add(food);
    
            if (foodDto.NutritionFacts != null)
            {
                var nutritionFacts = mapper.Map<NutritionFacts>(foodDto.NutritionFacts);
                nutritionFacts.Id = Guid.NewGuid();
                nutritionFacts.FoodId = food.Id;
                nutritionFacts.CreatedAt = DateTime.UtcNow;
                nutritionFacts.UpdatedAt = DateTime.UtcNow;
    
                dbContext.NutritionFacts.Add(nutritionFacts);
            }
            
            if (foodDto.AllergenIds?.Any() == true)
            {
                var foodAllergens = foodDto.AllergenIds.Select(allergenId => new FoodAllergen
                {
                    Id = Guid.NewGuid(),
                    FoodId = food.Id,
                    AllergenId = allergenId,
                    CreatedAt = DateTime.UtcNow
                }).ToList();
    
                dbContext.FoodAllergens.AddRange(foodAllergens);
            }
            
            var result = await dbContext.SaveChangesAsync();
            if (result == 0)
            {
                return BadRequest(new { Message = "Cannot save food to database" });
            }
            
            var createdFood = await dbContext.Foods
                .Include(x => x.Category)
                .Include(x => x.NutritionFacts)
                .Include(x => x.FoodAllergens)
                .ThenInclude(fa => fa.Allergen)
                .FirstOrDefaultAsync(x => x.Id == food.Id);
            
            return CreatedAtAction(nameof(GetFoodById),
                new { id = food.Id },
                mapper.Map<FoodDetailsDto>(createdFood));
        }
        catch (Exception e)
        {
            return StatusCode(500, new { Message = "There is some errors when creating food!" });
        }
    }
}