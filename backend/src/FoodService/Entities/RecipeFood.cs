using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodService.Entities;

[Table("recipe_foods")]
public class RecipeFood
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column("recipe_id")]
    public Guid RecipeId { get; set; }

    [Required]
    [Column("food_id")]
    public Guid FoodId { get; set; }

    [Column("quantity",TypeName = "decimal(8,2)")]
    public decimal Quantity { get; set; }

    [Column("unit")]
    [StringLength(50)]
    public string Unit { get; set; } = "g";

    [Column("order_index")]
    public int OrderIndex { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("FoodId")]
    public virtual Food Food { get; set; } = null!;

    // Recipe navigation would be added when Recipe entity is implemented
} 