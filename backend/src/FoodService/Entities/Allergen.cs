using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodService.Entities;

[Table("allergens")]
public class Allergen
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column("name_en")]
    [StringLength(100)]
    public string? NameEn { get; set; }

    [Column("name_vi")]
    [StringLength(100)]
    public string? NameVi { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("icon_url")]
    public string? IconUrl { get; set; }

    [Column("color_hex")]
    [StringLength(7)]
    public string? ColorHex { get; set; }

    [Column("category")]
    [StringLength(50)]
    public string? Category { get; set; }

    [Column("severity_level")]
    [StringLength(10)]
    public string SeverityLevel { get; set; } = "medium";

    [Column("is_common_in_vietnamese_food")]
    public bool IsCommonInVietnameseFood { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<FoodAllergen> FoodAllergens { get; set; } = new List<FoodAllergen>();
} 