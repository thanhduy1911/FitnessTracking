using Microsoft.EntityFrameworkCore;
using FoodService.Entities;

namespace FoodService.Data;

public class FoodDbContext(DbContextOptions<FoodDbContext> options) : DbContext(options)
{
    // DbSets
    public DbSet<FoodCategory> FoodCategories { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<NutritionFacts> NutritionFacts { get; set; }
    public DbSet<Allergen> Allergens { get; set; }
    public DbSet<FoodAllergen> FoodAllergens { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<FoodIngredient> FoodIngredients { get; set; }
    public DbSet<RecipeFood> RecipeFoods { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure FoodCategory
        modelBuilder.Entity<FoodCategory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ParentId);
            entity.HasIndex(e => e.Name);

            entity.HasOne(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e => e.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Foods)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure Food
        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.CategoryId);
            entity.HasIndex(e => e.Barcode).IsUnique();
            entity.HasIndex(e => e.DataSource);
            entity.HasIndex(e => e.VerificationStatus);

            // Configure serving size columns
            entity.Property(e => e.ServingSizeGrams)
                .HasColumnType("decimal(8,2)")
                .HasColumnName("serving_size_grams");
            
            entity.Property(e => e.ServingSizeDescription)
                .HasMaxLength(255)
                .HasColumnName("serving_size_description");
            
            entity.Property(e => e.ServingSizeDescriptionEn)
                .HasMaxLength(255)
                .HasColumnName("serving_size_description_en");
            
            entity.Property(e => e.AlternativeServingSizes)
                .HasColumnType("text")
                .HasColumnName("alternative_serving_sizes");

            entity.HasOne(e => e.Category)
                .WithMany(e => e.Foods)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.NutritionFacts)
                .WithOne(e => e.Food)
                .HasForeignKey<NutritionFacts>(e => e.FoodId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.FoodAllergens)
                .WithOne(e => e.Food)
                .HasForeignKey(e => e.FoodId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.FoodIngredients)
                .WithOne(e => e.Food)
                .HasForeignKey(e => e.FoodId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.RecipeFoods)
                .WithOne(e => e.Food)
                .HasForeignKey(e => e.FoodId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure NutritionFacts
        modelBuilder.Entity<NutritionFacts>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.FoodId).IsUnique();

            entity.HasOne(e => e.Food)
                .WithOne(e => e.NutritionFacts)
                .HasForeignKey<NutritionFacts>(e => e.FoodId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Allergen
        modelBuilder.Entity<Allergen>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name);

            entity.HasMany(e => e.FoodAllergens)
                .WithOne(e => e.Allergen)
                .HasForeignKey(e => e.AllergenId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure FoodAllergen
        modelBuilder.Entity<FoodAllergen>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.FoodId);
            entity.HasIndex(e => e.AllergenId);
            entity.HasIndex(e => new { e.FoodId, e.AllergenId }).IsUnique();

            entity.HasOne(e => e.Food)
                .WithMany(e => e.FoodAllergens)
                .HasForeignKey(e => e.FoodId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Allergen)
                .WithMany(e => e.FoodAllergens)
                .HasForeignKey(e => e.AllergenId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Ingredient
        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name);

            entity.HasMany(e => e.FoodIngredients)
                .WithOne(e => e.Ingredient)
                .HasForeignKey(e => e.IngredientId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure FoodIngredient
        modelBuilder.Entity<FoodIngredient>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.FoodId);
            entity.HasIndex(e => e.IngredientId);
            entity.HasIndex(e => new { e.FoodId, e.OrderIndex });

            entity.HasOne(e => e.Food)
                .WithMany(e => e.FoodIngredients)
                .HasForeignKey(e => e.FoodId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Ingredient)
                .WithMany(e => e.FoodIngredients)
                .HasForeignKey(e => e.IngredientId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure RecipeFood
        modelBuilder.Entity<RecipeFood>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.RecipeId);
            entity.HasIndex(e => e.FoodId);
            entity.HasIndex(e => new { e.RecipeId, e.OrderIndex });

            entity.HasOne(e => e.Food)
                .WithMany(e => e.RecipeFoods)
                .HasForeignKey(e => e.FoodId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure PostgreSQL specific settings
        ConfigurePostgreSql(modelBuilder);
    }

    private static void ConfigurePostgreSql(ModelBuilder modelBuilder)
    {
        // Configure UUID generation for PostgreSQL
        modelBuilder.Entity<FoodCategory>()
            .Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        modelBuilder.Entity<Food>()
            .Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        modelBuilder.Entity<NutritionFacts>()
            .Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        modelBuilder.Entity<Allergen>()
            .Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        modelBuilder.Entity<FoodAllergen>()
            .Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        modelBuilder.Entity<Ingredient>()
            .Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        modelBuilder.Entity<FoodIngredient>()
            .Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        modelBuilder.Entity<RecipeFood>()
            .Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        // Configure timestamp defaults
        modelBuilder.Entity<FoodCategory>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<FoodCategory>()
            .Property(e => e.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Food>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Food>()
            .Property(e => e.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<NutritionFacts>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<NutritionFacts>()
            .Property(e => e.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            switch (entry.Entity)
            {
                case FoodCategory foodCategory:
                {
                    if (entry.State == EntityState.Added)
                        foodCategory.CreatedAt = DateTime.UtcNow;
                    foodCategory.UpdatedAt = DateTime.UtcNow;
                    break;
                }
                case Food food:
                {
                    if (entry.State == EntityState.Added)
                        food.CreatedAt = DateTime.UtcNow;
                    food.UpdatedAt = DateTime.UtcNow;
                    break;
                }
                case NutritionFacts nutritionFacts:
                {
                    if (entry.State == EntityState.Added)
                        nutritionFacts.CreatedAt = DateTime.UtcNow;
                    nutritionFacts.UpdatedAt = DateTime.UtcNow;
                    break;
                }
            }
        }
    }
} 