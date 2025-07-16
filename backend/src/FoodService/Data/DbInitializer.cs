using Microsoft.EntityFrameworkCore;
using FoodService.Services;

namespace FoodService.Data;

public class DbInitializer
{
    public static async Task InitDbAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<DbInitializer>>();
        
        try
        {
            var context = services.GetRequiredService<FoodDbContext>();
            var seedService = services.GetRequiredService<FoodSeedService>();
            
            await SeedDataAsync(context, seedService, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            
            // Log more details about the error
            logger.LogError("Error details: {ErrorMessage}", ex.Message);
            
            // Don't throw in production, just log
            if (app.Environment.IsDevelopment())
            {
                throw;
            }
        }
    }

    private static async Task SeedDataAsync(FoodDbContext context, FoodSeedService seedService, ILogger logger)
    {
        logger.LogInformation("Starting database initialization...");
        
        try
        {
            // Check if database exists
            var canConnect = await context.Database.CanConnectAsync();
            if (!canConnect)
            {
                logger.LogInformation("Database doesn't exist. Creating database...");
                await context.Database.EnsureCreatedAsync();
            }
            
            // Check for pending migrations
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                logger.LogInformation("Applying {Count} pending migrations...", pendingMigrations.Count());
                await context.Database.MigrateAsync();
                logger.LogInformation("Migrations applied successfully.");
            }
            else
            {
                logger.LogInformation("No pending migrations found.");
            }

            // Check if already seeded
            var existingFoodCount = await context.Foods.CountAsync();
            if (existingFoodCount > 0)
            {
                logger.LogInformation("Database already contains {Count} food items - skipping seed.", existingFoodCount);
                return;
            }

            logger.LogInformation("Database is empty. Starting seed process...");
            
            // Seed the database
            await seedService.SeedDatabaseAsync();
            
            // Verify seeding
            var foodCount = await context.Foods.CountAsync();
            var categoryCount = await context.FoodCategories.CountAsync();
            var allergenCount = await context.Allergens.CountAsync();
            
            logger.LogInformation("Database seeding completed successfully:");
            logger.LogInformation("- {FoodCount} foods created", foodCount);
            logger.LogInformation("- {CategoryCount} categories created", categoryCount);
            logger.LogInformation("- {AllergenCount} allergens created", allergenCount);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during database initialization");
            throw;
        }
    }
}