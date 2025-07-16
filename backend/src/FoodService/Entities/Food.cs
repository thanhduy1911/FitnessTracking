using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodService.Entities;

[Table("foods")]
public class Food
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    [Column("name_en")]
    [StringLength(255)]
    public string NameEn { get; set; }

    [Column("name_vi")]
    [StringLength(255)]
    public string NameVi { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("food_code")]
    [StringLength(100)]
    public string? FoodCode { get; set; }

    [Column("barcode")]
    [StringLength(50)]
    public string Barcode { get; set; }

    [Column("category_id")]
    public Guid? CategoryId { get; set; }

    // Data source tracking
    [Required]
    [Column("data_source")]
    [StringLength(50)]
    public string DataSource { get; set; } = string.Empty;

    [Column("external_id")]
    [StringLength(100)]
    public string ExternalId { get; set; }

    [Column("source_url")]
    public string SourceUrl { get; set; }

    // Serving size information
    public decimal? ServingSizeGrams { get; set; }  // Standard serving size in grams
    public string ServingSizeDescription { get; set; }  // "1 bát", "1 quả", "1 ly"
    public string ServingSizeDescriptionEn { get; set; }  // "1 bowl", "1 piece", "1 cup"
    
    // Alternative serving sizes (JSON string)
    public string AlternativeServingSizes { get; set; }  // JSON: [{"size": 150, "description": "1 bát nhỏ"}, {"size": 250, "description": "1 bát lớn"}]

    // Status and verification
    [Column("is_verified")]
    public bool IsVerified { get; set; } = false;

    [Column("verification_status")]
    [StringLength(20)]
    public string VerificationStatus { get; set; } = "pending";

    [Column("verified_by")]
    public Guid? VerifiedBy { get; set; }

    [Column("verified_at")]
    public DateTime? VerifiedAt { get; set; }

    // Metadata
    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_by")]
    public Guid? CreatedBy { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("CategoryId")]
    public virtual FoodCategory Category { get; set; }

    public virtual NutritionFacts NutritionFacts { get; set; }

    public virtual ICollection<FoodAllergen> FoodAllergens { get; set; } = new List<FoodAllergen>();

    public virtual ICollection<FoodIngredient> FoodIngredients { get; set; } = new List<FoodIngredient>();

    public virtual ICollection<RecipeFood> RecipeFoods { get; set; } = new List<RecipeFood>();
}