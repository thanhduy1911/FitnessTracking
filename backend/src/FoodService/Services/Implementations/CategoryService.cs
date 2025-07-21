using FoodService.DTOs.Category;
using FoodService.DTOs.Common;
using FoodService.Entities;
using FoodService.Services.Interfaces;
using FoodService.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace FoodService.Services.Implementations;

public class CategoryService(FoodDbContext dbContext, IMapper mapper) : ICategoryService
{
    public async Task<PaginatedResponse<CategoryListDto>> GetCategoriesAsync(PaginationRequest request)
    {
        var query = dbContext.FoodCategories.AsQueryable();
        var total = await query.CountAsync();
        var items = await query
            .OrderBy(x => x.SortOrder)
            .Skip(request.Skip)
            .Take(request.PageSize)
            .ToListAsync();
        var dtos = items.Select(c => mapper.Map<CategoryListDto>(c)).ToList();
        return PaginatedResponse<CategoryListDto>.Create(dtos, total, request.Page, request.PageSize);
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
    {
        var entity = await dbContext.FoodCategories
            .Include(c => c.Children)
            .FirstOrDefaultAsync(c => c.Id == id);
        
        return entity == null ? null : mapper.Map<CategoryDto>(entity);
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
    {
        var entity = mapper.Map<FoodCategory>(dto);
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        dbContext.FoodCategories.Add(entity);
        await dbContext.SaveChangesAsync();
        return mapper.Map<CategoryDto>(entity);
    }

    public async Task<CategoryDto?> UpdateCategoryAsync(Guid id, UpdateCategoryDto dto)
    {
        var entity = await dbContext.FoodCategories.FindAsync(id);
        if (entity == null) return null;
        mapper.Map(dto, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
        return mapper.Map<CategoryDto>(entity);
    }

    public async Task<bool> DeleteCategoryAsync(Guid id)
    {
        var entity = await dbContext.FoodCategories.FindAsync(id);
        if (entity == null) return false;
        dbContext.FoodCategories.Remove(entity);
        await dbContext.SaveChangesAsync();
        return true;
    }
} 