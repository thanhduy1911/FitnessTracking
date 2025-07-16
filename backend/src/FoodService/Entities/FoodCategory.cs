using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodService.Entities;

[Table("food_categories")]
public class FoodCategory
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

    [Column("parent_id")]
    public Guid? ParentId { get; set; }

    [Column("sort_order")]
    public int SortOrder { get; set; } = 0;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("ParentId")]
    public virtual FoodCategory Parent { get; set; }

    public virtual ICollection<FoodCategory> Children { get; set; } = new List<FoodCategory>();

    public virtual ICollection<Food> Foods { get; set; } = new List<Food>();
} 