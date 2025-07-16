using AutoMapper;
using FoodService.Entities;
using FoodService.DTOs.Food;
using FoodService.DTOs.Nutrition;
using FoodService.DTOs.Category;
using FoodService.DTOs.Allergen;
using System.Text.Json;

namespace FoodService.Mapping;

public class FoodMappingProfile : Profile
{
    public FoodMappingProfile()
    {
        // Food Entity to DTOs
        CreateMap<Food, FoodListDto>()
            .ForMember(dest => dest.CaloriesKcal, opt => opt.MapFrom(src => src.NutritionFacts != null ? src.NutritionFacts.CaloriesKcal : null))
            .ForMember(dest => dest.ProteinG, opt => opt.MapFrom(src => src.NutritionFacts != null ? src.NutritionFacts.ProteinG : null))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.NameVi : string.Empty));

        CreateMap<Food, FoodDetailsDto>()
            .ForMember(dest => dest.AlternativeServingSizes, opt => opt.MapFrom(src => 
                !string.IsNullOrEmpty(src.AlternativeServingSizes) 
                    ? JsonSerializer.Deserialize<List<AlternativeServingDto>>(src.AlternativeServingSizes, (JsonSerializerOptions?)null) 
                    : new List<AlternativeServingDto>()))
            .ForMember(dest => dest.Allergens, opt => opt.MapFrom(src => src.FoodAllergens.Select(fa => fa.Allergen)));

        // DTOs to Food Entity
        CreateMap<CreateFoodDto, Food>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.AlternativeServingSizes, opt => opt.MapFrom(src => 
                src.AlternativeServingSizes.Any() ? JsonSerializer.Serialize(src.AlternativeServingSizes, (JsonSerializerOptions?)null) : null))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.NutritionFacts, opt => opt.Ignore())
            .ForMember(dest => dest.FoodAllergens, opt => opt.Ignore())
            .ForMember(dest => dest.FoodIngredients, opt => opt.Ignore())
            .ForMember(dest => dest.RecipeFoods, opt => opt.Ignore())
            .ForMember(dest => dest.VerifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.VerifiedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.VerificationStatus, opt => opt.MapFrom(src => "pending"));

        CreateMap<UpdateFoodDto, Food>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.AlternativeServingSizes, opt => opt.MapFrom(src => 
                src.AlternativeServingSizes != null && src.AlternativeServingSizes.Any() 
                    ? JsonSerializer.Serialize(src.AlternativeServingSizes, (JsonSerializerOptions?)null) 
                    : null))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.NutritionFacts, opt => opt.Ignore())
            .ForMember(dest => dest.FoodAllergens, opt => opt.Ignore())
            .ForMember(dest => dest.FoodIngredients, opt => opt.Ignore())
            .ForMember(dest => dest.RecipeFoods, opt => opt.Ignore())
            .ForMember(dest => dest.VerifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.VerifiedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.IsVerified, opt => opt.Ignore())
            .ForMember(dest => dest.VerificationStatus, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // AlternativeServingDto mapping
        CreateMap<AlternativeServingDto, AlternativeServingDto>();
    }
} 