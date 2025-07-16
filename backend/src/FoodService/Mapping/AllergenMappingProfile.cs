using AutoMapper;
using FoodService.Entities;
using FoodService.DTOs.Allergen;

namespace FoodService.Mapping;

public class AllergenMappingProfile : Profile
{
    public AllergenMappingProfile()
    {
        // Allergen Entity to DTOs
        CreateMap<Allergen, AllergenDto>()
            .ForMember(dest => dest.IconUrl, opt => opt.MapFrom(src => (string?)null))
            .ForMember(dest => dest.ColorHex, opt => opt.MapFrom(src => (string?)null))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => "general"))
            .ForMember(dest => dest.SeverityLevel, opt => opt.MapFrom(src => "medium"))
            .ForMember(dest => dest.IsCommonInVietnameseFood, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<Allergen, AllergenListDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NameVi ?? src.Name))
            .ForMember(dest => dest.IconUrl, opt => opt.MapFrom(src => (string?)null))
            .ForMember(dest => dest.ColorHex, opt => opt.MapFrom(src => (string?)null))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => "general"))
            .ForMember(dest => dest.SeverityLevel, opt => opt.MapFrom(src => "medium"))
            .ForMember(dest => dest.IsCommonInVietnameseFood, opt => opt.MapFrom(src => false));

        // DTOs to Allergen Entity
        CreateMap<CreateAllergenDto, Allergen>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.FoodAllergens, opt => opt.Ignore());

        CreateMap<UpdateAllergenDto, Allergen>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.FoodAllergens, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // FoodAllergen Entity to DTO
        CreateMap<FoodAllergen, FoodAllergenDto>()
            .ForMember(dest => dest.SeverityLevel, opt => opt.MapFrom(src => src.Severity))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => (string?)null));
    }
} 