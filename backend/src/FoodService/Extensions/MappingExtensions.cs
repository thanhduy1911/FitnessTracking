using AutoMapper;
using FoodService.Entities;
using FoodService.DTOs.Food;
using FoodService.DTOs.Nutrition;
using FoodService.DTOs.Category;
using FoodService.DTOs.Allergen;
using FoodService.DTOs.Common;
using System.Text.Json;

namespace FoodService.Extensions;

public static class MappingExtensions
{
    /// <summary>
    /// Map Food entity to FoodListDto with nutrition info
    /// </summary>
    public static FoodListDto ToFoodListDto(this Food food, IMapper mapper)
    {
        return mapper.Map<FoodListDto>(food);
    }

    /// <summary>
    /// Map Food entity to FoodDetailsDto with all relationships
    /// </summary>
    public static FoodDetailsDto ToFoodDetailsDto(this Food food, IMapper mapper)
    {
        return mapper.Map<FoodDetailsDto>(food);
    }

    /// <summary>
    /// Map collection of Food entities to paginated response
    /// </summary>
    public static PaginatedResponse<FoodListDto> ToPaginatedFoodListDto(
        this IEnumerable<Food> foods, 
        IMapper mapper, 
        int totalCount, 
        int pageNumber, 
        int pageSize)
    {
        var foodDtos = foods.Select(f => f.ToFoodListDto(mapper)).ToList();
        return PaginatedResponse<FoodListDto>.Create(foodDtos, totalCount, pageNumber, pageSize);
    }

    /// <summary>
    /// Map alternative serving sizes from JSON string to DTO list
    /// </summary>
    public static List<AlternativeServingDto> ParseAlternativeServingSizes(this string? jsonString)
    {
        if (string.IsNullOrEmpty(jsonString))
            return new List<AlternativeServingDto>();

        try
        {
            return JsonSerializer.Deserialize<List<AlternativeServingDto>>(jsonString, (JsonSerializerOptions?)null) ?? new List<AlternativeServingDto>();
        }
        catch
        {
            return new List<AlternativeServingDto>();
        }
    }

    /// <summary>
    /// Map alternative serving sizes to JSON string
    /// </summary>
    public static string? ToAlternativeServingSizesJson(this List<AlternativeServingDto>? servingSizes)
    {
        if (servingSizes == null || !servingSizes.Any())
            return null;

        try
        {
            return JsonSerializer.Serialize(servingSizes, (JsonSerializerOptions?)null);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Map NutritionFacts entity to DTO with data quality calculation
    /// </summary>
    public static NutritionFactsDto ToNutritionFactsDto(this NutritionFacts nutrition, IMapper mapper)
    {
        return mapper.Map<NutritionFactsDto>(nutrition);
    }

    /// <summary>
    /// Map FoodCategory entity to CategoryDto with food count
    /// </summary>
    public static CategoryDto ToCategoryDto(this FoodCategory category, IMapper mapper)
    {
        return mapper.Map<CategoryDto>(category);
    }

    /// <summary>
    /// Map FoodCategory entity to CategoryListDto for lightweight listings
    /// </summary>
    public static CategoryListDto ToCategoryListDto(this FoodCategory category, IMapper mapper)
    {
        return mapper.Map<CategoryListDto>(category);
    }

    /// <summary>
    /// Map collection of FoodCategory entities to hierarchical structure
    /// </summary>
    public static List<CategoryDto> ToHierarchicalCategoryDto(
        this IEnumerable<FoodCategory> categories, 
        IMapper mapper)
    {
        var categoryDtos = categories.Select(c => c.ToCategoryDto(mapper)).ToList();
        
        // Build hierarchical structure
        var rootCategories = categoryDtos.Where(c => c.ParentId == null).ToList();
        foreach (var root in rootCategories)
        {
            BuildCategoryHierarchy(root, categoryDtos);
        }
        
        return rootCategories;
    }

    private static void BuildCategoryHierarchy(CategoryDto parent, List<CategoryDto> allCategories)
    {
        parent.Children = allCategories.Where(c => c.ParentId == parent.Id).ToList();
        foreach (var child in parent.Children)
        {
            BuildCategoryHierarchy(child, allCategories);
        }
    }

    /// <summary>
    /// Map Allergen entity to AllergenDto
    /// </summary>
    public static AllergenDto ToAllergenDto(this Allergen allergen, IMapper mapper)
    {
        return mapper.Map<AllergenDto>(allergen);
    }

    /// <summary>
    /// Map Allergen entity to AllergenListDto
    /// </summary>
    public static AllergenListDto ToAllergenListDto(this Allergen allergen, IMapper mapper)
    {
        return mapper.Map<AllergenListDto>(allergen);
    }

    /// <summary>
    /// Map FoodAllergen entity to FoodAllergenDto
    /// </summary>
    public static FoodAllergenDto ToFoodAllergenDto(this FoodAllergen foodAllergen, IMapper mapper)
    {
        return mapper.Map<FoodAllergenDto>(foodAllergen);
    }

    /// <summary>
    /// Create API response with success data
    /// </summary>
    public static ApiResponse<T> ToApiResponse<T>(this T data, string message = "Thành công")
    {
        return ApiResponse<T>.SuccessResponse(data, message);
    }

    /// <summary>
    /// Create API response with error
    /// </summary>
    public static ApiResponse<T> ToErrorApiResponse<T>(this string message, List<string>? errors = null)
    {
        return ApiResponse<T>.ErrorResponse(message, errors);
    }

    /// <summary>
    /// Create paginated API response
    /// </summary>
    public static ApiResponse<PaginatedResponse<T>> ToPaginatedApiResponse<T>(
        this PaginatedResponse<T> paginatedData, 
        string message = "Thành công")
    {
        return ApiResponse<PaginatedResponse<T>>.SuccessResponse(paginatedData, message);
    }

    /// <summary>
    /// Validate and map CreateFoodDto to Food entity
    /// </summary>
    public static Food ToFoodEntity(this CreateFoodDto createDto, IMapper mapper)
    {
        var food = mapper.Map<Food>(createDto);
        
        // Set default values
        food.CreatedAt = DateTime.UtcNow;
        food.UpdatedAt = DateTime.UtcNow;
        food.IsActive = true;
        food.VerificationStatus = "pending";
        food.IsVerified = false;
        
        return food;
    }

    /// <summary>
    /// Update Food entity with UpdateFoodDto
    /// </summary>
    public static Food UpdateFromDto(this Food food, UpdateFoodDto updateDto, IMapper mapper)
    {
        mapper.Map(updateDto, food);
        food.UpdatedAt = DateTime.UtcNow;
        return food;
    }

    /// <summary>
    /// Map CreateNutritionFactsDto to NutritionFacts entity
    /// </summary>
    public static NutritionFacts ToNutritionFactsEntity(this CreateNutritionFactsDto createDto, IMapper mapper)
    {
        var nutrition = mapper.Map<NutritionFacts>(createDto);
        nutrition.CreatedAt = DateTime.UtcNow;
        nutrition.UpdatedAt = DateTime.UtcNow;
        return nutrition;
    }

    /// <summary>
    /// Update NutritionFacts entity with UpdateNutritionFactsDto
    /// </summary>
    public static NutritionFacts UpdateFromDto(this NutritionFacts nutrition, UpdateNutritionFactsDto updateDto, IMapper mapper)
    {
        mapper.Map(updateDto, nutrition);
        nutrition.UpdatedAt = DateTime.UtcNow;
        return nutrition;
    }
} 