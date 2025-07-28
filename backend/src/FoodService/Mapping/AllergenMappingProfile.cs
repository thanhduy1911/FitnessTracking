using AutoMapper;
using FoodService.Entities;
using FoodService.DTOs.Allergen;

namespace FoodService.Mapping;

/// <summary>
/// AutoMapper profile for Allergen entity and DTOs
/// </summary>
public class AllergenMappingProfile : Profile
{
    public AllergenMappingProfile()
    {
        // Allergen Entity to DTOs
        CreateMap<Allergen, AllergenDto>()
            .ForMember(dest => dest.FoodCount, opt => opt.MapFrom(src => src.FoodAllergens.Count));

        CreateMap<Allergen, AllergenListDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NameVi ?? src.Name));

        // DTOs to Allergen Entity
        CreateMap<CreateAllergenDto, Allergen>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.FoodAllergens, opt => opt.Ignore());

        CreateMap<UpdateAllergenDto, Allergen>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.FoodAllergens, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // FoodAllergen Entity to DTO
        CreateMap<FoodAllergen, FoodAllergenDto>();
    }
} 