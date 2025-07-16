using AutoMapper;
using FoodService.Entities;
using FoodService.DTOs.Category;

namespace FoodService.Mapping;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        // FoodCategory Entity to DTOs
        CreateMap<FoodCategory, CategoryDto>()
            .ForMember(dest => dest.IconUrl, opt => opt.MapFrom(src => (string?)null))
            .ForMember(dest => dest.ColorHex, opt => opt.MapFrom(src => (string?)null))
            .ForMember(dest => dest.FoodCount, opt => opt.MapFrom(src => src.Foods.Count))
            .ForMember(dest => dest.TotalFoodCount, opt => opt.MapFrom(src => 
                src.Foods.Count + src.Children.Sum(c => c.Foods.Count)));

        CreateMap<FoodCategory, CategoryListDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NameVi))
            .ForMember(dest => dest.IconUrl, opt => opt.MapFrom(src => (string?)null))
            .ForMember(dest => dest.ColorHex, opt => opt.MapFrom(src => (string?)null))
            .ForMember(dest => dest.FoodCount, opt => opt.MapFrom(src => src.Foods.Count));

        // DTOs to FoodCategory Entity
        CreateMap<CreateCategoryDto, FoodCategory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Parent, opt => opt.Ignore())
            .ForMember(dest => dest.Children, opt => opt.Ignore())
            .ForMember(dest => dest.Foods, opt => opt.Ignore());

        CreateMap<UpdateCategoryDto, FoodCategory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Parent, opt => opt.Ignore())
            .ForMember(dest => dest.Children, opt => opt.Ignore())
            .ForMember(dest => dest.Foods, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
} 