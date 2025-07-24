using AutoMapper;
using FoodService.Data;
using FoodService.DTOs.Common;
using FoodService.DTOs.Nutrition;
using FoodService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodService.Services.Implementations;

public class NutritionFactsService(FoodDbContext context, IMapper mapper) : INutritionFactsService
{
    public async Task<PaginatedResponse<NutritionFactsDto>> GetAllNutritionAsync(PaginationRequest request)
    {
        var query = context.NutritionFacts.AsQueryable();
        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(n => n.UpdatedAt)
            .Skip(request.Skip)
            .Take(request.PageSize)
            .ToListAsync();
        var dtos = mapper.Map<List<NutritionFactsDto>>(items);
        return PaginatedResponse<NutritionFactsDto>.Create(dtos, total, request.Page, request.PageSize);
    }

    public async Task<NutritionFactsDto> GetNutritionByIdAsync(Guid id)
    {
        var entity = await context.NutritionFacts.FirstOrDefaultAsync(n => n.Id == id);
        return (entity == null ? null : mapper.Map<NutritionFactsDto>(entity)) ?? throw new InvalidOperationException();
    }

    public async Task<NutritionFactsDto> CreateNutritionAsync(CreateNutritionFactsDto nutritionFactsDto)
    {
        // Kiểm tra đã có NutritionFacts cho foodId chưa
        var exists = await context.NutritionFacts.AnyAsync(n => n.FoodId == nutritionFactsDto.FoodId);
        if (exists)
            throw new InvalidOperationException("Đã tồn tại NutritionFacts cho món ăn này");
        var entity = mapper.Map<Entities.NutritionFacts>(nutritionFactsDto);
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        context.NutritionFacts.Add(entity);
        await context.SaveChangesAsync();
        return mapper.Map<NutritionFactsDto>(entity);
    }

    public async Task<NutritionFactsDto?> UpdateNutritionAsync(Guid id, UpdateNutritionFactsDto updateNutritionFactsDto)
    {
        var entity = await context.NutritionFacts.FirstOrDefaultAsync(n => n.Id == id);
        if (entity == null) return null;
        // Không update FoodId
        mapper.Map(updateNutritionFactsDto, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return mapper.Map<NutritionFactsDto>(entity);
    }

    public async Task<bool> DeleteNutritionAsync(Guid id)
    {
        var entity = await context.NutritionFacts.FirstOrDefaultAsync(n => n.Id == id);
        if (entity == null) return false;
        context.NutritionFacts.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}