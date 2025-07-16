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

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<FoodAllergen> FoodAllergens { get; set; } = new List<FoodAllergen>();
} 