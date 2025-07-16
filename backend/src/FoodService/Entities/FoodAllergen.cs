using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodService.Entities;

[Table("food_allergens")]
public class FoodAllergen
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column("food_id")]
    public Guid FoodId { get; set; }

    [Required]
    [Column("allergen_id")]
    public Guid AllergenId { get; set; }

    [Column("severity")]
    [StringLength(20)]
    public string Severity { get; set; } = "contains"; // contains, may_contain, traces

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("FoodId")]
    public virtual Food Food { get; set; } = null!;

    [ForeignKey("AllergenId")]
    public virtual Allergen Allergen { get; set; } = null!;
} 