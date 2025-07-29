using AutoMapper;
using FoodService.Data;
using FoodService.DTOs.Allergen;
using FoodService.DTOs.Common;
using FoodService.Entities;
using FoodService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodService.Services.Implementations;

public class AllergenService(FoodDbContext dbContext, IMapper mapper) : IAllergenService
{
    public async Task<PaginatedResponse<AllergenListDto>> GetAllergensAsync(PaginationRequest request)
    {
        var query = dbContext.Allergens
            .Include(a => a.FoodAllergens)
            .ThenInclude(a => a.Food)
            .Where(a => a.IsActive);
        
        // Apply sorting
        query = ApplySorting(query, request.SortBy, request.SortDirection);
        
        // Get total count
        var totalItems = await query.CountAsync();
        
        // Apply pagination
        var allergens = await query
            .Skip(request.Skip)
            .Take(request.PageSize)
            .ToListAsync();
        
        var allergenDtos = mapper.Map<List<AllergenListDto>>(allergens);
        
        return PaginatedResponse<AllergenListDto>.Create(
            allergenDtos,
            totalItems,
            request.Page,
            request.PageSize
        );
    }

    /// <summary>
    /// Get allergen by ID with complete details
    /// </summary>
    public async Task<AllergenDto?> GetAllergenByIdAsync(Guid id)
    {
        var allergen = await dbContext.Allergens
            .Include(a => a.FoodAllergens)
            .ThenInclude(fa => fa.Food)
            .FirstOrDefaultAsync(a => a.Id == id);
        
        if (allergen == null)
            return null;
        
        var dto = mapper.Map<AllergenDto>(allergen);
        return dto;
    }

    public async Task<AllergenDto> CreateAllergenAsync(CreateAllergenDto createAllergenDto)
    {
        var allergen = mapper.Map<Allergen>(createAllergenDto);
        allergen.CreatedAt = DateTime.UtcNow;
        allergen.UpdatedAt = DateTime.UtcNow;
        
        dbContext.Allergens.Add(allergen);
        await dbContext.SaveChangesAsync();
        
        var createdDto = mapper.Map<AllergenDto>(allergen);
        return createdDto;
    }

    public async Task<AllergenDto?> UpdateAllergenAsync(Guid id, UpdateAllergenDto updateAllergenDto)
    {
        var allergen = await dbContext.Allergens.FindAsync(id);
        if (allergen == null)
            return null;
        
        // Update only non-null properties
        if (!string.IsNullOrWhiteSpace(updateAllergenDto.Name))
            allergen.Name = updateAllergenDto.Name;
        
        if (updateAllergenDto.NameEn != null)
            allergen.NameEn = updateAllergenDto.NameEn;
        
        if (updateAllergenDto.NameVi != null)
            allergen.NameVi = updateAllergenDto.NameVi;
        
        if (updateAllergenDto.Description != null)
            allergen.Description = updateAllergenDto.Description;
        
        if (updateAllergenDto.IconUrl != null)
            allergen.IconUrl = updateAllergenDto.IconUrl;
        
        if (updateAllergenDto.ColorHex != null)
            allergen.ColorHex = updateAllergenDto.ColorHex;
        
        if (updateAllergenDto.Category != null)
            allergen.Category = updateAllergenDto.Category;
        
        if (!string.IsNullOrWhiteSpace(updateAllergenDto.SeverityLevel))
            allergen.SeverityLevel = updateAllergenDto.SeverityLevel;
        
        if (updateAllergenDto.IsCommonInVietnameseFood.HasValue)
            allergen.IsCommonInVietnameseFood = updateAllergenDto.IsCommonInVietnameseFood.Value;
        
        if (updateAllergenDto.IsActive.HasValue)
            allergen.IsActive = updateAllergenDto.IsActive.Value;
        
        allergen.UpdatedAt = DateTime.UtcNow;
        
        await dbContext.SaveChangesAsync();
        
        var updatedDto = mapper.Map<AllergenDto>(allergen);
        return updatedDto;
    }

    public async Task<bool> DeleteAllergenAsync(Guid id)
    {
        var allergen = await dbContext.Allergens.FindAsync(id);
        if (allergen == null)
            return false;
        
        allergen.IsActive = false; // Soft delete
        allergen.UpdatedAt = DateTime.UtcNow;
        
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<FoodAllergenDto?> GetAllergenWithFoodByFoodIdAsync(Guid foodId)
    {
        var foodAllergen = await dbContext.FoodAllergens
            .Include(fa => fa.Allergen)
            .Include(fa => fa.Food)
            .FirstOrDefaultAsync(fa => fa.FoodId == foodId);
        
        if (foodAllergen == null)
            return null;
        
        var dto = mapper.Map<FoodAllergenDto>(foodAllergen);
        return dto;
    }
    
    private static IQueryable<Allergen> ApplySorting(IQueryable<Allergen> query, string? requestSortBy, string requestSortDirection)
    {
        if (string.IsNullOrWhiteSpace(requestSortBy))
        {
            return query.OrderBy(a => a.Name);
        }
        
        var isDescending = requestSortDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase);

        return requestSortBy.ToLower() switch
        {
            "name" => isDescending ? query.OrderByDescending(a => a.Name) : query.OrderBy(a => a.Name),
            "nameen" => isDescending ? query.OrderByDescending(a => a.NameEn) : query.OrderBy(a => a.NameEn),
            "namevi" => isDescending ?  query.OrderByDescending(a => a.NameVi) : query.OrderBy(a => a.NameVi),
            "description" => isDescending ? query.OrderByDescending(a => a.Description) : query.OrderBy(a => a.Description),
            "createat" => isDescending ? query.OrderByDescending(a => a.CreatedAt) : query.OrderBy(a => a.CreatedAt),
            _ => query.OrderBy(a => a.Name)
        };
    }
}