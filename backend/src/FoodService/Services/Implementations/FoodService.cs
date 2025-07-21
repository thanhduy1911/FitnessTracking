using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FoodService.Data;
using FoodService.Entities;
using FoodService.DTOs.Food;
using FoodService.DTOs.Common;
using FoodService.Services.Interfaces;

namespace FoodService.Services.Implementations;

/// <summary>
/// Implementation of IFoodService
/// Provides CRUD operations and business logic for Food entities
/// </summary>
public class FoodService(FoodDbContext context, IMapper mapper) : IFoodService
{
    /// <summary>
    /// Get food by ID with complete details
    /// </summary>
    public async Task<FoodDetailsDto?> GetFoodByIdAsync(Guid id)
    {
        var food = await context.Foods
            .Include(f => f.Category)
            .Include(f => f.NutritionFacts)
            .Include(f => f.FoodAllergens)
                .ThenInclude(fa => fa.Allergen)
            .FirstOrDefaultAsync(f => f.Id == id && f.IsActive);

        return food == null ? null : mapper.Map<FoodDetailsDto>(food);
    }

    /// <summary>
    /// Get paginated list of foods
    /// </summary>
    public async Task<PaginatedResponse<FoodListDto>> GetFoodsAsync(PaginationRequest request)
    {
        var query = context.Foods
            .Include(f => f.Category)
            .Include(f => f.NutritionFacts)
            .Where(f => f.IsActive);

        // Apply sorting
        query = ApplySorting(query, request.SortBy, request.SortDirection);

        // Get total count
        var totalItems = await query.CountAsync();

        // Apply pagination
        var foods = await query
            .Skip(request.Skip)
            .Take(request.PageSize)
            .ToListAsync();

        var foodDtos = mapper.Map<List<FoodListDto>>(foods);

        return PaginatedResponse<FoodListDto>.Create(
            foodDtos, 
            totalItems, 
            request.Page, 
            request.PageSize
        );
    }

    /// <summary>
    /// Search foods by query and filters
    /// </summary>
    public async Task<PaginatedResponse<FoodListDto>> SearchFoodsAsync(FoodSearchRequest request)
    {
        var query = context.Foods
            .Include(f => f.Category)
            .Include(f => f.NutritionFacts)
            .AsQueryable();

        // Apply active filter
        if (request.OnlyActive == true)
            query = query.Where(f => f.IsActive);

        // Apply search query
        if (!string.IsNullOrWhiteSpace(request.Query))
        {
            var searchTerm = request.Query.ToLower();
            query = query.Where(f => 
                f.Description != null && f.NameVi != null && f.NameEn != null && f.Barcode != null && (f.Name.ToLower().Contains(searchTerm) ||
                    f.NameEn.ToLower().Contains(searchTerm) ||
                    f.NameVi.ToLower().Contains(searchTerm) ||
                    f.Description.ToLower().Contains(searchTerm) ||
                    f.Barcode.Contains(searchTerm)));
        }

        // Apply category filter
        if (request.CategoryId.HasValue)
            query = query.Where(f => f.CategoryId == request.CategoryId);

        // Apply data source filter
        if (!string.IsNullOrWhiteSpace(request.DataSource))
            query = query.Where(f => f.DataSource == request.DataSource);

        // Apply verification status filter
        if (!string.IsNullOrWhiteSpace(request.VerificationStatus))
            query = query.Where(f => f.VerificationStatus == request.VerificationStatus);

        // Apply verified filter
        if (request.OnlyVerified == true)
            query = query.Where(f => f.IsVerified);

        // Apply nutrition filters
        if (request.MinCalories.HasValue)
            query = query.Where(f => f.NutritionFacts != null && f.NutritionFacts.CaloriesKcal >= request.MinCalories);

        if (request.MaxCalories.HasValue)
            query = query.Where(f => f.NutritionFacts != null && f.NutritionFacts.CaloriesKcal <= request.MaxCalories);

        if (request.MinProtein.HasValue)
            query = query.Where(f => f.NutritionFacts != null && f.NutritionFacts.ProteinG >= request.MinProtein);

        if (request.MaxProtein.HasValue)
            query = query.Where(f => f.NutritionFacts != null && f.NutritionFacts.ProteinG <= request.MaxProtein);

        // Apply allergen filters
        if (request.ExcludeAllergens?.Any() == true)
        {
            query = query.Where(f => !f.FoodAllergens.Any(fa => request.ExcludeAllergens.Contains(fa.AllergenId)));
        }

        if (request.IncludeAllergens?.Any() == true)
        {
            query = query.Where(f => f.FoodAllergens.Any(fa => request.IncludeAllergens.Contains(fa.AllergenId)));
        }

        // Apply sorting
        query = ApplySorting(query, request.SortBy, request.SortDirection);

        // Get total count
        var totalItems = await query.CountAsync();

        // Apply pagination
        var foods = await query
            .Skip(request.Skip)
            .Take(request.PageSize)
            .ToListAsync();

        var foodDtos = mapper.Map<List<FoodListDto>>(foods);

        return PaginatedResponse<FoodListDto>.Create(
            foodDtos, 
            totalItems, 
            request.Page, 
            request.PageSize
        );
    }

    /// <summary>
    /// Create new food item
    /// </summary>
    public async Task<FoodDetailsDto> CreateFoodAsync(CreateFoodDto dto)
    {
        var food = mapper.Map<Food>(dto);
        food.Id = Guid.NewGuid();
        food.CreatedAt = DateTime.UtcNow;
        food.UpdatedAt = DateTime.UtcNow;

        context.Foods.Add(food);

        // Add nutrition facts if provided
        if (dto.NutritionFacts != null)
        {
            var nutritionFacts = mapper.Map<NutritionFacts>(dto.NutritionFacts);
            nutritionFacts.Id = Guid.NewGuid();
            nutritionFacts.FoodId = food.Id;
            nutritionFacts.CreatedAt = DateTime.UtcNow;
            nutritionFacts.UpdatedAt = DateTime.UtcNow;

            context.NutritionFacts.Add(nutritionFacts);
        }

        // Add allergen relationships
        if (dto.AllergenIds.Count != 0 == true)
        {
            var foodAllergens = dto.AllergenIds.Select(allergenId => new FoodAllergen
            {
                Id = Guid.NewGuid(),
                FoodId = food.Id,
                AllergenId = allergenId,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            context.FoodAllergens.AddRange(foodAllergens);
        }

        await context.SaveChangesAsync();

        // Return created food with all relationships
        return await GetFoodByIdAsync(food.Id) 
            ?? throw new InvalidOperationException("Failed to retrieve created food");
    }

    /// <summary>
    /// Update existing food item
    /// </summary>
    public async Task<FoodDetailsDto?> UpdateFoodAsync(Guid id, UpdateFoodDto dto)
    {
        var food = await context.Foods
            .Include(f => f.NutritionFacts)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (food == null)
            return null;

        // Update food properties
        mapper.Map(dto, food);
        food.UpdatedAt = DateTime.UtcNow;

        // Update nutrition facts if provided
        if (dto.NutritionFacts != null)
        {
            if (food.NutritionFacts == null)
            {
                // Create new nutrition facts
                var nutritionFacts = mapper.Map<NutritionFacts>(dto.NutritionFacts);
                nutritionFacts.Id = Guid.NewGuid();
                nutritionFacts.FoodId = food.Id;
                nutritionFacts.CreatedAt = DateTime.UtcNow;
                nutritionFacts.UpdatedAt = DateTime.UtcNow;

                context.NutritionFacts.Add(nutritionFacts);
            }
            else
            {
                // Update existing nutrition facts
                mapper.Map(dto.NutritionFacts, food.NutritionFacts);
                food.NutritionFacts.UpdatedAt = DateTime.UtcNow;
            }
        }

        // Update allergen relationships if provided
        if (dto.AllergenIds != null)
        {
            // Remove existing allergen relationships
            var existingAllergens = await context.FoodAllergens
                .Where(fa => fa.FoodId == id)
                .ToListAsync();

            context.FoodAllergens.RemoveRange(existingAllergens);

            // Add new allergen relationships
            if (dto.AllergenIds.Any())
            {
                var foodAllergens = dto.AllergenIds.Select(allergenId => new FoodAllergen
                {
                    Id = Guid.NewGuid(),
                    FoodId = food.Id,
                    AllergenId = allergenId,
                    CreatedAt = DateTime.UtcNow
                }).ToList();

                context.FoodAllergens.AddRange(foodAllergens);
            }
        }

        await context.SaveChangesAsync();

        // Return updated food with all relationships
        return await GetFoodByIdAsync(id);
    }

    /// <summary>
    /// Delete food item (soft delete)
    /// </summary>
    public async Task<bool> DeleteFoodAsync(Guid id)
    {
        var food = await context.Foods.FirstOrDefaultAsync(f => f.Id == id);

        if (food == null)
            return false;

        // Soft delete
        food.IsActive = false;
        food.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Verify food item by admin/moderator
    /// </summary>
    public async Task<bool> VerifyFoodAsync(Guid id, Guid verifiedBy)
    {
        var food = await context.Foods.FirstOrDefaultAsync(f => f.Id == id);

        if (food == null)
            return false;

        food.IsVerified = true;
        food.VerificationStatus = "approved";
        food.VerifiedBy = verifiedBy;
        food.VerifiedAt = DateTime.UtcNow;
        food.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Get foods by category
    /// </summary>
    public async Task<List<FoodListDto>> GetFoodsByCategoryAsync(Guid categoryId)
    {
        var foods = await context.Foods
            .Include(f => f.Category)
            .Include(f => f.NutritionFacts)
            .Where(f => f.CategoryId == categoryId && f.IsActive)
            .OrderBy(f => f.Name)
            .ToListAsync();

        return mapper.Map<List<FoodListDto>>(foods);
    }

    /// <summary>
    /// Get foods by allergen (foods that contain specific allergen)
    /// </summary>
    public async Task<List<FoodListDto>> GetFoodsByAllergenAsync(Guid allergenId)
    {
        var foods = await context.Foods
            .Include(f => f.Category)
            .Include(f => f.NutritionFacts)
            .Where(f => f.FoodAllergens.Any(fa => fa.AllergenId == allergenId) && f.IsActive)
            .OrderBy(f => f.Name)
            .ToListAsync();

        return mapper.Map<List<FoodListDto>>(foods);
    }

    /// <summary>
    /// Apply sorting to query
    /// </summary>
    private IQueryable<Food> ApplySorting(IQueryable<Food> query, string? sortBy, string sortDirection)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
        {
            return query.OrderBy(f => f.Name);
        }

        var isDescending = sortDirection.ToLower() == "desc";

        return sortBy.ToLower() switch
        {
            "name" => isDescending ? query.OrderByDescending(f => f.Name) : query.OrderBy(f => f.Name),
            "namevi" => isDescending ? query.OrderByDescending(f => f.NameVi) : query.OrderBy(f => f.NameVi),
            "nameen" => isDescending ? query.OrderByDescending(f => f.NameEn) : query.OrderBy(f => f.NameEn),
            "calories" => isDescending ? query.OrderByDescending(f => f.NutritionFacts!.CaloriesKcal) : query.OrderBy(f => f.NutritionFacts!.CaloriesKcal),
            "protein" => isDescending ? query.OrderByDescending(f => f.NutritionFacts!.ProteinG) : query.OrderBy(f => f.NutritionFacts!.ProteinG),
            "createdat" => isDescending ? query.OrderByDescending(f => f.CreatedAt) : query.OrderBy(f => f.CreatedAt),
            "updatedat" => isDescending ? query.OrderByDescending(f => f.UpdatedAt) : query.OrderBy(f => f.UpdatedAt),
            _ => query.OrderBy(f => f.Name)
        };
    }
} 