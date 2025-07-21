using System.ComponentModel.DataAnnotations;

namespace FoodService.DTOs.Category;

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