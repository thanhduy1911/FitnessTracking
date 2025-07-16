using FoodService.Mapping;

namespace FoodService.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register AutoMapper with all mapping profiles
    /// </summary>
    public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(config => 
        {
            config.AddProfile<FoodMappingProfile>();
            config.AddProfile<NutritionMappingProfile>();
            config.AddProfile<CategoryMappingProfile>();
            config.AddProfile<AllergenMappingProfile>();
        });
        
        return services;
    }

    /// <summary>
    /// Register all mapping profiles individually for better control
    /// </summary>
    public static IServiceCollection AddMappingProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<FoodMappingProfile>();
            cfg.AddProfile<NutritionMappingProfile>();
            cfg.AddProfile<CategoryMappingProfile>();
            cfg.AddProfile<AllergenMappingProfile>();
        });
        
        return services;
    }

    /// <summary>
    /// Register AutoMapper with configuration options
    /// </summary>
    public static IServiceCollection AddAutoMapperWithConfiguration(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            // Add all mapping profiles
            cfg.AddProfile<FoodMappingProfile>();
            cfg.AddProfile<NutritionMappingProfile>();
            cfg.AddProfile<CategoryMappingProfile>();
            cfg.AddProfile<AllergenMappingProfile>();
            
            // Configuration options
            cfg.AllowNullDestinationValues = true;
            cfg.AllowNullCollections = true;
            
            // Custom value resolvers and converters can be added here
        });
        
        return services;
    }
} 