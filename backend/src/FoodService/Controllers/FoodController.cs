using AutoMapper;
using AutoMapper.QueryableExtensions;
using FoodService.Data;
using FoodService.DTOs.Food;
using FoodService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodService.Controllers;

[ApiController]
[Route("api/foods")]
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
        var food = mapper.Map<Food>(foodDto);
        dbContext.Foods.Add(food);
        
        var newFood = mapper.Map<CreateFoodDto>(food);
        if (newFood == null) return NotFound();
        
        var result = await dbContext.SaveChangesAsync() > 0;
        if (!result) return BadRequest("Could not save changes to the database");
        
        return CreatedAtAction(nameof(GetFoodById), 
            new { id = food.Id }, mapper.Map<FoodDetailsDto>(food));
    }
}