using AutoMapper;
using FoodService.Entities;
using FoodService.DTOs.Nutrition;

namespace FoodService.Mapping;

public class NutritionMappingProfile : Profile
{
    public NutritionMappingProfile()
    {
        // NutritionFacts Entity to DTOs
        CreateMap<NutritionFacts, NutritionFactsDto>()
            .ForMember(dest => dest.CarbsG, opt => opt.MapFrom(src => src.CarbohydrateG))
            .ForMember(dest => dest.VitaminB1Mg, opt => opt.MapFrom(src => src.ThiamineMg))
            .ForMember(dest => dest.VitaminB2Mg, opt => opt.MapFrom(src => src.RiboflavinMg))
            .ForMember(dest => dest.VitaminB3Mg, opt => opt.MapFrom(src => src.NiacinMg))
            .ForMember(dest => dest.DataQuality, opt => opt.MapFrom(src => 
                src.ConfidenceScore >= 0.9m ? "high" : 
                src.ConfidenceScore >= 0.7m ? "medium" : "low"))
            .ForMember(dest => dest.LastVerified, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.Notes, opt => opt.Ignore());

        // DTOs to NutritionFacts Entity
        CreateMap<CreateNutritionFactsDto, NutritionFacts>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CarbohydrateG, opt => opt.MapFrom(src => src.CarbsG))
            .ForMember(dest => dest.ThiamineMg, opt => opt.MapFrom(src => src.VitaminB1Mg))
            .ForMember(dest => dest.RiboflavinMg, opt => opt.MapFrom(src => src.VitaminB2Mg))
            .ForMember(dest => dest.NiacinMg, opt => opt.MapFrom(src => src.VitaminB3Mg))
            .ForMember(dest => dest.ConfidenceScore, opt => opt.MapFrom(src => 
                src.DataQuality == "high" ? 0.9m : 
                src.DataQuality == "medium" ? 0.7m : 0.5m))
            .ForMember(dest => dest.ServingSizeG, opt => opt.MapFrom(src => 100m))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Food, opt => opt.Ignore())
            // Map các fields không có trong DTO với null
            .ForMember(dest => dest.CopperMg, opt => opt.MapFrom(src => (decimal?)null))
            .ForMember(dest => dest.ManganeseMg, opt => opt.MapFrom(src => (decimal?)null))
            .ForMember(dest => dest.SeleniumMcg, opt => opt.MapFrom(src => (decimal?)null))
            .ForMember(dest => dest.BiotinMcg, opt => opt.MapFrom(src => (decimal?)null))
            .ForMember(dest => dest.PantothenicAcidMg, opt => opt.MapFrom(src => (decimal?)null))
            .ForMember(dest => dest.CholineMg, opt => opt.MapFrom(src => (decimal?)null));

        CreateMap<UpdateNutritionFactsDto, NutritionFacts>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.FoodId, opt => opt.Ignore())
            .ForMember(dest => dest.CarbohydrateG, opt => opt.MapFrom(src => src.CarbsG))
            .ForMember(dest => dest.ThiamineMg, opt => opt.MapFrom(src => src.VitaminB1Mg))
            .ForMember(dest => dest.RiboflavinMg, opt => opt.MapFrom(src => src.VitaminB2Mg))
            .ForMember(dest => dest.NiacinMg, opt => opt.MapFrom(src => src.VitaminB3Mg))
            .ForMember(dest => dest.ConfidenceScore, opt => opt.MapFrom(src => 
                src.DataQuality == "high" ? 0.9m : 
                src.DataQuality == "medium" ? 0.7m : 
                src.DataQuality == "low" ? 0.5m : (decimal?)null))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Food, opt => opt.Ignore())
            .ForMember(dest => dest.ServingSizeG, opt => opt.Ignore())
            .ForMember(dest => dest.CopperMg, opt => opt.Ignore())
            .ForMember(dest => dest.ManganeseMg, opt => opt.Ignore())
            .ForMember(dest => dest.SeleniumMcg, opt => opt.Ignore())
            .ForMember(dest => dest.BiotinMcg, opt => opt.Ignore())
            .ForMember(dest => dest.PantothenicAcidMg, opt => opt.Ignore())
            .ForMember(dest => dest.CholineMg, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
} 