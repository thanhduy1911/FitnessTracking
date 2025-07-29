using System.ComponentModel.DataAnnotations;

namespace FoodService.DTOs.Allergen;

/// <summary>
/// DTO for creating new allergens
/// </summary>
public class CreateAllergenDto
{
    [Required(ErrorMessage = "Tên allergen là bắt buộc")]
    [StringLength(100, ErrorMessage = "Tên allergen không được vượt quá 100 ký tự")]
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
    
    [Required(ErrorMessage = "Category là bắt buộc")]
    [StringLength(50, ErrorMessage = "Category không được vượt quá 50 ký tự")]
    public string Category { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Severity level là bắt buộc")]
    [RegularExpression(@"^(low|medium|high)$", ErrorMessage = "Severity level phải là 'low', 'medium', hoặc 'high'")]
    public string SeverityLevel { get; set; } = "medium";
    
    public bool IsCommonInVietnameseFood { get; set; }
    public bool IsActive { get; set; } = true;
}