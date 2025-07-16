using FoodService.Data;
using FoodService.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodService.Services;

public class FoodSeedService(FoodDbContext context)
{
    public async Task SeedDatabaseAsync()
    {
        // Check if already seeded
        if (await context.Foods.AnyAsync(f => f.DataSource == "manual_seed"))
        {
            return; // Already seeded
        }

        // Seed categories first
        await SeedCategoriesAsync();

        // Seed allergens
        await SeedAllergensAsync();

        // Seed foods with nutrition facts
        await SeedFoodsAsync();

        await context.SaveChangesAsync();
    }

    private async Task SeedCategoriesAsync()
    {
        var categories = new[]
        {
            new FoodCategory
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Staple Foods",
                NameEn = "Staple Foods",
                NameVi = "Thực phẩm chủ yếu",
                Description = "Rice, noodles, bread and other staple foods",
                SortOrder = 1
            },
            new FoodCategory
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Vegetables",
                NameEn = "Vegetables",
                NameVi = "Rau củ",
                Description = "Fresh vegetables and leafy greens",
                SortOrder = 2
            },
            new FoodCategory
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Fruits",
                NameEn = "Fruits",
                NameVi = "Trái cây",
                Description = "Fresh fruits and tropical fruits",
                SortOrder = 3
            },
            new FoodCategory
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Name = "Protein Sources",
                NameEn = "Protein Sources",
                NameVi = "Thực phẩm đạm",
                Description = "Meat, fish, eggs, and legumes",
                SortOrder = 4
            },
            new FoodCategory
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                Name = "Condiments & Seasonings",
                NameEn = "Condiments & Seasonings",
                NameVi = "Gia vị",
                Description = "Spices, sauces, and seasonings",
                SortOrder = 5
            },
            new FoodCategory
            {
                Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                Name = "Beverages",
                NameEn = "Beverages",
                NameVi = "Đồ uống",
                Description = "Drinks and beverages",
                SortOrder = 6
            }
        };

        await context.FoodCategories.AddRangeAsync(categories);
    }

    private async Task SeedAllergensAsync()
    {
        var allergens = new[]
        {
            new Allergen { 
                Name = "Gluten", 
                NameEn = "Gluten", 
                NameVi = "Gluten",
                Description = "Protein found in wheat, barley, and rye that can cause digestive issues in sensitive individuals"
            },
            new Allergen { 
                Name = "Dairy", 
                NameEn = "Dairy", 
                NameVi = "Sữa",
                Description = "Milk and milk products that may cause lactose intolerance or milk allergy reactions"
            },
            new Allergen { 
                Name = "Eggs", 
                NameEn = "Eggs", 
                NameVi = "Trứng",
                Description = "Chicken eggs or egg-derived ingredients that can trigger allergic reactions"
            },
            new Allergen { 
                Name = "Fish", 
                NameEn = "Fish", 
                NameVi = "Cá",
                Description = "Fish and fish products that may cause allergic reactions in sensitive individuals"
            },
            new Allergen { 
                Name = "Shellfish", 
                NameEn = "Shellfish", 
                NameVi = "Hải sản",
                Description = "Crustaceans and mollusks that are common allergens in seafood"
            },
            new Allergen { 
                Name = "Peanuts", 
                NameEn = "Peanuts", 
                NameVi = "Đậu phộng",
                Description = "Peanuts and peanut-derived products that can cause severe allergic reactions"
            },
            new Allergen { 
                Name = "Tree Nuts", 
                NameEn = "Tree Nuts", 
                NameVi = "Hạt cây",
                Description = "Tree nuts such as almonds, walnuts, cashews that may trigger allergic reactions"
            },
            new Allergen { 
                Name = "Soy", 
                NameEn = "Soy", 
                NameVi = "Đậu nành",
                Description = "Soybeans and soy-derived products that can cause allergic reactions"
            },
            new Allergen { 
                Name = "Sesame", 
                NameEn = "Sesame", 
                NameVi = "Mè",
                Description = "Sesame seeds and sesame-derived products that may cause allergic reactions"
            }
        };

        await context.Allergens.AddRangeAsync(allergens);
    }

    private async Task SeedFoodsAsync()
    {
        var foods = new List<(Food Food, NutritionFacts Nutrition)>
        {
            // STAPLE FOODS
            (
                new Food
                {
                    Name = "Cơm tẻ",
                    NameEn = "Steamed white rice",
                    NameVi = "Cơm tẻ",
                    Description = "Cơm tẻ trắng được nấu chín từ gạo tẻ, là thực phẩm chính trong bữa ăn hàng ngày của người Việt Nam",
                    FoodCode = "VN001",
                    CategoryId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    DataSource = "manual_seed",
                    ExternalId = "VN001",
                    Barcode = "VN001",
                    SourceUrl = "https://manual-seed.local/VN001",
                    ServingSizeGrams = 150,
                    ServingSizeDescription = "1 bát",
                    ServingSizeDescriptionEn = "1 bowl",
                    AlternativeServingSizes = "[{\"grams\":100,\"description\":\"1 phần nhỏ\",\"descriptionEn\":\"1 small portion\"},{\"grams\":200,\"description\":\"1 bát lớn\",\"descriptionEn\":\"1 large bowl\"}]",
                    IsVerified = true,
                    VerificationStatus = "approved"
                },
                new NutritionFacts
                {
                    CaloriesKcal = 130,
                    ProteinG = 2.7m,
                    FatG = 0.3m,
                    CarbohydrateG = 28.0m,
                    FiberG = 0.4m,
                    SodiumMg = 5,
                    CalciumMg = 28,
                    IronMg = 0.8m,
                    VitaminCMg = 0,
                    VitaminAMcg = 0,
                    DataSource = "manual_seed",
                    ConfidenceScore = 0.9m
                }
            ),
            (
                new Food
                {
                    Name = "Bánh mì",
                    NameEn = "Vietnamese baguette",
                    NameVi = "Bánh mì",
                    Description = "Bánh mì Việt Nam với vỏ ngoài giòn, ruột mềm, thường dùng để làm bánh mì kẹp hoặc ăn cùng món khác",
                    FoodCode = "VN002",
                    CategoryId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    DataSource = "manual_seed",
                    ExternalId = "VN002",
                    Barcode = "VN002",
                    SourceUrl = "https://manual-seed.local/VN002",
                    ServingSizeGrams = 80,
                    ServingSizeDescription = "1 ổ",
                    ServingSizeDescriptionEn = "1 piece",
                    AlternativeServingSizes = "[{\"grams\":60,\"description\":\"1 ổ nhỏ\",\"descriptionEn\":\"1 small piece\"},{\"grams\":100,\"description\":\"1 ổ lớn\",\"descriptionEn\":\"1 large piece\"}]",
                    IsVerified = true,
                    VerificationStatus = "approved"
                },
                new NutritionFacts
                {
                    CaloriesKcal = 265,
                    ProteinG = 9.0m,
                    FatG = 3.2m,
                    CarbohydrateG = 49.0m,
                    FiberG = 2.7m,
                    SodiumMg = 491,
                    CalciumMg = 77,
                    IronMg = 3.6m,
                    VitaminCMg = 0,
                    VitaminAMcg = 0,
                    DataSource = "manual_seed",
                    ConfidenceScore = 0.9m
                }
            ),
            (
                new Food
                {
                    Name = "Bún",
                    NameEn = "Rice vermicelli",
                    NameVi = "Bún",
                    Description = "Bún tươi làm từ bột gạo, mềm mại, thường dùng trong các món bún bò, bún chả, bún riêu",
                    FoodCode = "VN003",
                    CategoryId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    DataSource = "manual_seed",
                    ExternalId = "VN003",
                    Barcode = "VN003",
                    SourceUrl = "https://manual-seed.local/VN003",
                    ServingSizeGrams = 200,
                    ServingSizeDescription = "1 bát",
                    ServingSizeDescriptionEn = "1 bowl",
                    AlternativeServingSizes = "[{\"grams\":150,\"description\":\"1 bát nhỏ\",\"descriptionEn\":\"1 small bowl\"},{\"grams\":250,\"description\":\"1 bát lớn\",\"descriptionEn\":\"1 large bowl\"}]",
                    IsVerified = true,
                    VerificationStatus = "approved"
                },
                new NutritionFacts
                {
                    CaloriesKcal = 109,
                    ProteinG = 0.89m,
                    FatG = 0.56m,
                    CarbohydrateG = 25.0m,
                    FiberG = 1.8m,
                    SodiumMg = 3,
                    CalciumMg = 3,
                    IronMg = 0.2m,
                    VitaminCMg = 0,
                    VitaminAMcg = 0,
                    DataSource = "manual_seed",
                    ConfidenceScore = 0.9m
                }
            ),

            // VEGETABLES
            (
                new Food
                {
                    Name = "Rau muống",
                    NameEn = "Water spinach",
                    NameVi = "Rau muống",
                    Description = "Rau muống tươi, giàu vitamin và khoáng chất, thường được xào với tỏi hoặc luộc chấm nước mắm",
                    FoodCode = "VN004",
                    CategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    DataSource = "manual_seed",
                    ExternalId = "VN004",
                    Barcode = "VN004",
                    SourceUrl = "https://manual-seed.local/VN004",
                    ServingSizeGrams = 100,
                    ServingSizeDescription = "1 dĩa",
                    ServingSizeDescriptionEn = "1 plate",
                    AlternativeServingSizes = "[{\"grams\":150,\"description\":\"1 dĩa lớn\",\"descriptionEn\":\"1 large plate\"},{\"grams\":200,\"description\":\"1 bó\",\"descriptionEn\":\"1 bundle\"}]",
                    IsVerified = true,
                    VerificationStatus = "approved"
                },
                new NutritionFacts
                {
                    CaloriesKcal = 19,
                    ProteinG = 2.6m,
                    FatG = 0.2m,
                    CarbohydrateG = 3.14m,
                    FiberG = 2.1m,
                    SodiumMg = 113,
                    CalciumMg = 77,
                    IronMg = 1.67m,
                    VitaminCMg = 55,
                    VitaminAMcg = 316,
                    DataSource = "manual_seed",
                    ConfidenceScore = 0.9m
                }
            ),
            (
                new Food
                {
                    Name = "Cà chua",
                    NameEn = "Tomato",
                    NameVi = "Cà chua",
                    Description = "Cà chua tươi màu đỏ, giàu vitamin C và lycopene, thường dùng trong salad, nấu canh hoặc làm nước sốt",
                    FoodCode = "VN005",
                    CategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    DataSource = "manual_seed",
                    ExternalId = "VN005",
                    Barcode = "VN005",
                    SourceUrl = "https://manual-seed.local/VN005",
                    ServingSizeGrams = 120,
                    ServingSizeDescription = "1 quả",
                    ServingSizeDescriptionEn = "1 piece",
                    AlternativeServingSizes = "[{\"grams\":80,\"description\":\"1 quả nhỏ\",\"descriptionEn\":\"1 small piece\"},{\"grams\":150,\"description\":\"1 quả lớn\",\"descriptionEn\":\"1 large piece\"}]",
                    IsVerified = true,
                    VerificationStatus = "approved"
                },
                new NutritionFacts
                {
                    CaloriesKcal = 18,
                    ProteinG = 0.88m,
                    FatG = 0.2m,
                    CarbohydrateG = 3.89m,
                    FiberG = 1.2m,
                    SodiumMg = 5,
                    CalciumMg = 10,
                    IronMg = 0.27m,
                    VitaminCMg = 14,
                    VitaminAMcg = 42,
                    DataSource = "manual_seed",
                    ConfidenceScore = 0.9m
                }
            ),

            // FRUITS
            (
                new Food
                {
                    Name = "Chuối",
                    NameEn = "Banana",
                    NameVi = "Chuối",
                    Description = "Chuối chín tự nhiên, ngọt mát, giàu kali và vitamin B6, thường ăn trực tiếp hoặc làm bánh",
                    FoodCode = "VN006",
                    CategoryId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    DataSource = "manual_seed",
                    ExternalId = "VN006",
                    Barcode = "VN006",
                    SourceUrl = "https://manual-seed.local/VN006",
                    ServingSizeGrams = 120,
                    ServingSizeDescription = "1 quả",
                    ServingSizeDescriptionEn = "1 piece",
                    AlternativeServingSizes = "[{\"grams\":90,\"description\":\"1 quả nhỏ\",\"descriptionEn\":\"1 small piece\"},{\"grams\":150,\"description\":\"1 quả lớn\",\"descriptionEn\":\"1 large piece\"}]",
                    IsVerified = true,
                    VerificationStatus = "approved"
                },
                new NutritionFacts
                {
                    CaloriesKcal = 89,
                    ProteinG = 1.09m,
                    FatG = 0.33m,
                    CarbohydrateG = 22.84m,
                    FiberG = 2.6m,
                    SodiumMg = 1,
                    CalciumMg = 5,
                    IronMg = 0.26m,
                    VitaminCMg = 8.7m,
                    VitaminAMcg = 3,
                    DataSource = "manual_seed",
                    ConfidenceScore = 0.9m
                }
            ),
            (
                new Food
                {
                    Name = "Cam",
                    NameEn = "Orange",
                    NameVi = "Cam",
                    Description = "Cam tươi ngọt mọng nước, giàu vitamin C và chất xơ, thường ăn tươi hoặc vắt nước cam",
                    FoodCode = "VN007",
                    CategoryId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    DataSource = "manual_seed",
                    ExternalId = "VN007",
                    Barcode = "VN007",
                    SourceUrl = "https://manual-seed.local/VN007",
                    ServingSizeGrams = 150,
                    ServingSizeDescription = "1 quả",
                    ServingSizeDescriptionEn = "1 piece",
                    AlternativeServingSizes = "[{\"grams\":120,\"description\":\"1 quả nhỏ\",\"descriptionEn\":\"1 small piece\"},{\"grams\":180,\"description\":\"1 quả lớn\",\"descriptionEn\":\"1 large piece\"}]",
                    IsVerified = true,
                    VerificationStatus = "approved"
                },
                new NutritionFacts
                {
                    CaloriesKcal = 47,
                    ProteinG = 0.94m,
                    FatG = 0.12m,
                    CarbohydrateG = 11.75m,
                    FiberG = 2.4m,
                    SodiumMg = 0,
                    CalciumMg = 40,
                    IronMg = 0.1m,
                    VitaminCMg = 53.2m,
                    VitaminAMcg = 11,
                    DataSource = "manual_seed",
                    ConfidenceScore = 0.9m
                }
            ),

            // PROTEIN SOURCES
            (
                new Food
                {
                    Name = "Thịt heo",
                    NameEn = "Pork",
                    NameVi = "Thịt heo",
                    Description = "Thịt heo tươi, giàu protein và chất béo, thường được nấu theo nhiều cách như luộc, nướng, xào",
                    FoodCode = "VN008",
                    CategoryId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    DataSource = "manual_seed",
                    ExternalId = "VN008",
                    Barcode = "VN008",
                    SourceUrl = "https://manual-seed.local/VN008",
                    ServingSizeGrams = 100,
                    ServingSizeDescription = "1 miếng",
                    ServingSizeDescriptionEn = "1 piece",
                    AlternativeServingSizes = "[{\"grams\":80,\"description\":\"1 miếng nhỏ\",\"descriptionEn\":\"1 small piece\"},{\"grams\":150,\"description\":\"1 miếng lớn\",\"descriptionEn\":\"1 large piece\"}]",
                    IsVerified = true,
                    VerificationStatus = "approved"
                },
                new NutritionFacts
                {
                    CaloriesKcal = 297,
                    ProteinG = 25.7m,
                    FatG = 20.8m,
                    CarbohydrateG = 0,
                    FiberG = 0,
                    SodiumMg = 62,
                    CalciumMg = 19,
                    IronMg = 0.87m,
                    VitaminCMg = 0.7m,
                    VitaminAMcg = 2,
                    DataSource = "manual_seed",
                    ConfidenceScore = 0.9m
                }
            ),
            (
                new Food
                {
                    Name = "Cá tra",
                    NameEn = "Pangasius",
                    NameVi = "Cá tra",
                    Description = "Cá tra tươi, thịt trắng, ít xương, giàu protein và omega-3, thường được nướng, luộc hoặc nấu canh",
                    FoodCode = "VN009",
                    CategoryId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    DataSource = "manual_seed",
                    ExternalId = "VN009",
                    Barcode = "VN009",
                    SourceUrl = "https://manual-seed.local/VN009",
                    ServingSizeGrams = 150,
                    ServingSizeDescription = "1 khúc",
                    ServingSizeDescriptionEn = "1 piece",
                    AlternativeServingSizes = "[{\"grams\":120,\"description\":\"1 khúc nhỏ\",\"descriptionEn\":\"1 small piece\"},{\"grams\":200,\"description\":\"1 khúc lớn\",\"descriptionEn\":\"1 large piece\"}]",
                    IsVerified = true,
                    VerificationStatus = "approved"
                },
                new NutritionFacts
                {
                    CaloriesKcal = 90,
                    ProteinG = 15.1m,
                    FatG = 3.0m,
                    CarbohydrateG = 0,
                    FiberG = 0,
                    SodiumMg = 89,
                    CalciumMg = 11,
                    IronMg = 0.4m,
                    VitaminCMg = 0,
                    VitaminAMcg = 54,
                    DataSource = "manual_seed",
                    ConfidenceScore = 0.9m
                }
            ),
            (
                new Food
                {
                    Name = "Trứng gà",
                    NameEn = "Chicken eggs",
                    NameVi = "Trứng gà",
                    Description = "Trứng gà tươi, nguồn protein hoàn chỉnh, thường được luộc, chiên, hoặc dùng làm nguyên liệu nấu ăn",
                    FoodCode = "VN010",
                    CategoryId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    DataSource = "manual_seed",
                    ExternalId = "VN010",
                    Barcode = "VN010",
                    SourceUrl = "https://manual-seed.local/VN010",
                    ServingSizeGrams = 60,
                    ServingSizeDescription = "1 quả",
                    ServingSizeDescriptionEn = "1 egg",
                    AlternativeServingSizes = "[{\"grams\":50,\"description\":\"1 quả nhỏ\",\"descriptionEn\":\"1 small egg\"},{\"grams\":70,\"description\":\"1 quả lớn\",\"descriptionEn\":\"1 large egg\"},{\"grams\":120,\"description\":\"2 quả\",\"descriptionEn\":\"2 eggs\"}]",
                    IsVerified = true,
                    VerificationStatus = "approved"
                },
                new NutritionFacts
                {
                    CaloriesKcal = 155,
                    ProteinG = 13.0m,
                    FatG = 11.0m,
                    CarbohydrateG = 1.1m,
                    FiberG = 0,
                    SodiumMg = 124,
                    CalciumMg = 50,
                    IronMg = 1.2m,
                    VitaminCMg = 0,
                    VitaminAMcg = 140,
                    DataSource = "manual_seed",
                    ConfidenceScore = 0.9m
                }
            )
        };

        foreach (var (food, nutrition) in foods)
        {
            await context.Foods.AddAsync(food);
            nutrition.FoodId = food.Id;
            await context.NutritionFacts.AddAsync(nutrition);
        }
    }
} 