using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodService.Entities;

[Table("food_ingredients")]
public class FoodIngredient
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column("food_id")]
    public Guid FoodId { get; set; }

    [Required]
    [Column("ingredient_id")]
    public Guid IngredientId { get; set; }

    [Column("order_index")]
    public int? OrderIndex { get; set; }
    
    [Column("percentage",TypeName = "decimal(5,2)")]
    public decimal? Percentage { get; set; }

    [Column("is_main_ingredient")]
    public bool IsMainIngredient { get; set; } = false;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("FoodId")]
    public virtual Food Food { get; set; } = null!;

    [ForeignKey("IngredientId")]
    public virtual Ingredient Ingredient { get; set; } = null!;
} 