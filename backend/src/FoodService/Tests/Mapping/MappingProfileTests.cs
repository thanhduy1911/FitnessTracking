using AutoMapper;
using FoodService.Entities;
using FoodService.DTOs.Food;
using FoodService.DTOs.Nutrition;
using FoodService.DTOs.Category;
using FoodService.DTOs.Allergen;
using FoodService.Mapping;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FoodService.Tests.Mapping;

public class MappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly MapperConfiguration _configuration;

    public MappingProfileTests()
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        
        _configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<FoodMappingProfile>();
            cfg.AddProfile<NutritionMappingProfile>();
            cfg.AddProfile<CategoryMappingProfile>();
            cfg.AddProfile<AllergenMappingProfile>();
        }, loggerFactory);

        _mapper = _configuration.CreateMapper();
    }

    [Fact]
    public void Configuration_IsValid()
    {
        // Test that all mapping configurations are valid
        _configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void Food_To_FoodListDto_ShouldMap_Successfully()
    {
        // Arrange
        var food = new Food
        {
            Id = Guid.NewGuid(),
            Name = "Cơm tấm",
            NameVi = "Cơm tấm",
            NameEn = "Broken Rice",
            ServingSizeGrams = 250,
            ServingSizeDescription = "1 đĩa",
            IsVerified = true,
            DataSource = "manual",
            UpdatedAt = DateTime.UtcNow,
            NutritionFacts = new NutritionFacts
            {
                CaloriesKcal = 350,
                ProteinG = 12
            },
            Category = new FoodCategory
            {
                NameVi = "Cơm"
            }
        };

        // Act
        var result = _mapper.Map<FoodListDto>(food);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(food.Id, result.Id);
        Assert.Equal(food.NameVi, result.NameVi);
        Assert.Equal(food.NameEn, result.NameEn);
        Assert.Equal(food.ServingSizeGrams, result.ServingSizeGrams);
        Assert.Equal(food.ServingSizeDescription, result.ServingSizeDescription);
        Assert.Equal(350, result.CaloriesKcal);
        Assert.Equal(12, result.ProteinG);
        Assert.Equal("Cơm", result.CategoryName);
        Assert.Equal(food.IsVerified, result.IsVerified);
        Assert.Equal(food.DataSource, result.DataSource);
        Assert.Equal(food.UpdatedAt, result.UpdatedAt);
    }

    [Fact]
    public void NutritionFacts_To_NutritionFactsDto_ShouldMap_Successfully()
    {
        // Arrange
        var nutritionFacts = new NutritionFacts
        {
            Id = Guid.NewGuid(),
            FoodId = Guid.NewGuid(),
            CaloriesKcal = 250,
            ProteinG = 15,
            CarbohydrateG = 30,
            FatG = 8,
            ConfidenceScore = 0.85m,
            DataSource = "USDA",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<NutritionFactsDto>(nutritionFacts);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(nutritionFacts.Id, result.Id);
        Assert.Equal(nutritionFacts.FoodId, result.FoodId);
        Assert.Equal(nutritionFacts.CaloriesKcal, result.CaloriesKcal);
        Assert.Equal(nutritionFacts.ProteinG, result.ProteinG);
        Assert.Equal(nutritionFacts.CarbohydrateG, result.CarbsG);
        Assert.Equal(nutritionFacts.FatG, result.FatG);
        Assert.Equal("medium", result.DataQuality); // 0.85 should be "medium"
        Assert.Equal(nutritionFacts.DataSource, result.DataSource);
        Assert.Equal(nutritionFacts.UpdatedAt, result.LastVerified);
    }

    [Fact]
    public void FoodCategory_To_CategoryDto_ShouldMap_Successfully()
    {
        // Arrange
        var category = new FoodCategory
        {
            Id = Guid.NewGuid(),
            Name = "Cơm",
            NameEn = "Rice",
            NameVi = "Cơm",
            Description = "Các món cơm",
            SortOrder = 1,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Foods = new List<Food>
            {
                new Food { Id = Guid.NewGuid(), Name = "Cơm tấm" },
                new Food { Id = Guid.NewGuid(), Name = "Cơm chiên" }
            },
            Children = new List<FoodCategory>()
        };

        // Act
        var result = _mapper.Map<CategoryDto>(category);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(category.Id, result.Id);
        Assert.Equal(category.Name, result.Name);
        Assert.Equal(category.NameEn, result.NameEn);
        Assert.Equal(category.NameVi, result.NameVi);
        Assert.Equal(category.Description, result.Description);
        Assert.Equal(category.SortOrder, result.SortOrder);
        Assert.Equal(category.IsActive, result.IsActive);
        Assert.Equal(2, result.FoodCount);
        Assert.Equal(2, result.TotalFoodCount);
    }

    [Fact]
    public void Allergen_To_AllergenDto_ShouldMap_Successfully()
    {
        // Arrange
        var allergen = new Allergen
        {
            Id = Guid.NewGuid(),
            Name = "Tôm",
            NameEn = "Shrimp",
            NameVi = "Tôm",
            Description = "Allergen từ tôm và các loại hải sản giáp xác",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            FoodAllergens = new List<FoodAllergen>()
        };

        // Act
        var result = _mapper.Map<AllergenDto>(allergen);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(allergen.Id, result.Id);
        Assert.Equal(allergen.Name, result.Name);
        Assert.Equal(allergen.NameEn, result.NameEn);
        Assert.Equal(allergen.NameVi, result.NameVi);
        Assert.Equal(allergen.Description, result.Description);
        Assert.Equal(allergen.IsActive, result.IsActive);
        Assert.Equal(allergen.CreatedAt, result.CreatedAt);
        Assert.Equal("general", result.Category);
        Assert.Equal("medium", result.SeverityLevel);
        Assert.False(result.IsCommonInVietnameseFood);
    }
} 