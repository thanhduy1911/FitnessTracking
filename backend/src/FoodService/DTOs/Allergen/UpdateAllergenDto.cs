using System.ComponentModel.DataAnnotations;

namespace FoodService.DTOs.Allergen;

/// <summary>
/// DTO for updating allergens
/// </summary>
public class UpdateAllergenDto
{
    [StringLength(100, ErrorMessage = "Tên allergen không được vượt quá 100 ký tự")]
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
    
    [StringLength(50, ErrorMessage = "Category không được vượt quá 50 ký tự")]
    public string? Category { get; set; }
    
    [RegularExpression(@"^(low|medium|high)$", ErrorMessage = "Severity level phải là 'low', 'medium', hoặc 'high'")]
    public string? SeverityLevel { get; set; }
    
    public bool? IsCommonInVietnameseFood { get; set; }
    public bool? IsActive { get; set; }
}