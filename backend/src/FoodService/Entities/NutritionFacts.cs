using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodService.Entities;

[Table("nutrition_facts")]
public class NutritionFacts
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column("food_id")]
    public Guid FoodId { get; set; }
    
    [Column("serving_size_g",TypeName = "decimal(8,2)")]
    public decimal? ServingSizeG { get; set; } = 100; // Default per 100g

    // Macronutrients
    [Column("calories_kcal",TypeName = "decimal(8,2)")]
    public decimal? CaloriesKcal { get; set; }
    
    [Column("protein_g",TypeName = "decimal(8,2)")]
    public decimal? ProteinG { get; set; }
    
    [Column("fat_g",TypeName = "decimal(8,2)")]
    public decimal? FatG { get; set; }
    
    [Column("carbohydrate_g", TypeName = "decimal(8,2)")]
    public decimal? CarbohydrateG { get; set; }
    
    [Column("fiber_g", TypeName = "decimal(8,2)")]
    public decimal? FiberG { get; set; }
    
    [Column("sugar_g",TypeName = "decimal(8,2)")]
    public decimal? SugarG { get; set; }

    // Detailed fats
    [Column("saturated_fat_g",TypeName = "decimal(8,2)")]
    public decimal? SaturatedFatG { get; set; }
    
    [Column("monounsaturated_fat_g",TypeName = "decimal(8,2)")]
    public decimal? MonounsaturatedFatG { get; set; }
    
    [Column("polyunsaturated_fat_g",TypeName = "decimal(8,2)")]
    public decimal? PolyunsaturatedFatG { get; set; }
    
    [Column("trans_fat_g",TypeName = "decimal(8,2)")]
    public decimal? TransFatG { get; set; }
    
    [Column("cholesterol_mg",TypeName = "decimal(8,2)")]
    public decimal? CholesterolMg { get; set; }

    // Minerals
    [Column("sodium_mg",TypeName = "decimal(8,2)")]
    public decimal? SodiumMg { get; set; }
    
    [Column("potassium_mg",TypeName = "decimal(8,2)")]
    public decimal? PotassiumMg { get; set; }
    
    [Column("calcium_mg",TypeName = "decimal(8,2)")]
    public decimal? CalciumMg { get; set; }

    [Column("iron_mg",TypeName = "decimal(8,2)")]
    public decimal? IronMg { get; set; }

    [Column("magnesium_mg",TypeName = "decimal(8,2)")]
    public decimal? MagnesiumMg { get; set; }

    [Column("phosphorus_mg",TypeName = "decimal(8,2)")]
    public decimal? PhosphorusMg { get; set; }
    
    [Column("zinc_mg",TypeName = "decimal(8,2)")]
    public decimal? ZincMg { get; set; }

    [Column("copper_mg",TypeName = "decimal(8,2)")]
    public decimal? CopperMg { get; set; }

    [Column("manganese_mg",TypeName = "decimal(8,2)")]
    public decimal? ManganeseMg { get; set; }

    [Column("selenium_mcg",TypeName = "decimal(8,2)")]
    public decimal? SeleniumMcg { get; set; }

    // Vitamins
    [Column("vitamin_a_mcg",TypeName = "decimal(8,2)")]
    public decimal? VitaminAMcg { get; set; }

    [Column("vitamin_c_mg",TypeName = "decimal(8,2)")]
    public decimal? VitaminCMg { get; set; }

    [Column("vitamin_d_mcg",TypeName = "decimal(8,2)")]
    public decimal? VitaminDMcg { get; set; }

    [Column("vitamin_e_mg",TypeName = "decimal(8,2)")]
    public decimal? VitaminEMg { get; set; }

    [Column("vitamin_k_mcg",TypeName = "decimal(8,2)")]
    public decimal? VitaminKMcg { get; set; }

    [Column("thiamine_mg",TypeName = "decimal(8,2)")]
    public decimal? ThiamineMg { get; set; } // Vitamin B1

    [Column("riboflavin_mg",TypeName = "decimal(8,2)")]
    public decimal? RiboflavinMg { get; set; } // Vitamin B2

    [Column("niacin_mg",TypeName = "decimal(8,2)")]
    public decimal? NiacinMg { get; set; } // Vitamin B3

    [Column("vitamin_b6_mg",TypeName = "decimal(8,2)")]
    public decimal? VitaminB6Mg { get; set; }

    [Column("folate_mcg",TypeName = "decimal(8,2)")]
    public decimal? FolateMcg { get; set; }

    [Column("vitamin_b12_mcg",TypeName = "decimal(8,2)")]
    public decimal? VitaminB12Mcg { get; set; }

    [Column("biotin_mcg",TypeName = "decimal(8,2)")]
    public decimal? BiotinMcg { get; set; }

    [Column("pantothenic_acid_mg",TypeName = "decimal(8,2)")]
    public decimal? PantothenicAcidMg { get; set; }

    [Column("choline_mg",TypeName = "decimal(8,2)")]
    public decimal? CholineMg { get; set; }

    // Metadata
    [Column("data_source")]
    [StringLength(50)]
    public string? DataSource { get; set; }

    [Column("confidence_score",TypeName = "decimal(3,2)")]
    public decimal? ConfidenceScore { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("FoodId")]
    public virtual Food Food { get; set; } = null!;
} 