using System.ComponentModel.DataAnnotations;

namespace FoodService.DTOs.Category;

/// <summary>
/// Category DTO for hierarchical food categories
/// </summary>
public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string NameVi { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public string? ColorHex { get; set; }
    
    // Hierarchical structure
    public Guid? ParentId { get; set; }
    public CategoryDto? Parent { get; set; }
    public List<CategoryDto> Children { get; set; } = new();
    
    // Display order
    public int SortOrder { get; set; }
    
    // Metadata
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Statistics
    public int FoodCount { get; set; }
    public int TotalFoodCount { get; set; } // Include children
}

/// <summary>
/// Lightweight category DTO for listings
/// </summary>
public class CategoryListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameVi { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public string? ColorHex { get; set; }
    public Guid? ParentId { get; set; }
    public int SortOrder { get; set; }
    public int FoodCount { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// DTO for creating new categories
/// </summary>
public class CreateCategoryDto
{
    [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
    [StringLength(100, ErrorMessage = "Tên danh mục không được vượt quá 100 ký tự")]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(100, ErrorMessage = "Tên tiếng Anh không được vượt quá 100 ký tự")]
    public string NameEn { get; set; } = string.Empty;
    
    [StringLength(100, ErrorMessage = "Tên tiếng Việt không được vượt quá 100 ký tự")]
    public string NameVi { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    [Url(ErrorMessage = "Icon URL phải là URL hợp lệ")]
    public string? IconUrl { get; set; }
    
    [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Color hex phải có format #RRGGBB hoặc #RGB")]
    public string? ColorHex { get; set; }
    
    public Guid? ParentId { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// DTO for updating categories
/// </summary>
public class UpdateCategoryDto
{
    [StringLength(100, ErrorMessage = "Tên danh mục không được vượt quá 100 ký tự")]
    public string? Name { get; set; }
    
    [StringLength(100, ErrorMessage = "Tên tiếng Anh không được vượt quá 100 ký tự")]
    public string? NameEn { get; set; }
    
    [StringLength(100, ErrorMessage = "Tên tiếng Việt không được vượt quá 100 ký tự")]
    public string? NameVi { get; set; }
    
    public string? Description { get; set; }
    
    [Url(ErrorMessage = "Icon URL phải là URL hợp lệ")]
    public string? IconUrl { get; set; }
    
    [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Color hex phải có format #RRGGBB hoặc #RGB")]
    public string? ColorHex { get; set; }
    
    public Guid? ParentId { get; set; }
    public int? SortOrder { get; set; }
    public bool? IsActive { get; set; }
} 