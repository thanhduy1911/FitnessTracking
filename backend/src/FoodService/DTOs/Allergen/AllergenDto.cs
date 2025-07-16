using System.ComponentModel.DataAnnotations;

namespace FoodService.DTOs.Allergen;

/// <summary>
/// Allergen DTO for allergen information
/// </summary>
public class AllergenDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string NameVi { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public string? ColorHex { get; set; }
    
    // Allergen classification
    public string Category { get; set; } = string.Empty; // "protein", "grain", "dairy", "fruit", etc.
    public string SeverityLevel { get; set; } = "medium"; // "low", "medium", "high"
    
    // Common in Vietnamese foods
    public bool IsCommonInVietnameseFood { get; set; }
    
    // Metadata
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Lightweight allergen DTO for listings
/// </summary>
public class AllergenListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameVi { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public string? ColorHex { get; set; }
    public string Category { get; set; } = string.Empty;
    public string SeverityLevel { get; set; } = "medium";
    public bool IsCommonInVietnameseFood { get; set; }
    public bool IsActive { get; set; } = true;
}

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

/// <summary>
/// Food-Allergen relationship DTO
/// </summary>
public class FoodAllergenDto
{
    public Guid FoodId { get; set; }
    public Guid AllergenId { get; set; }
    public AllergenDto Allergen { get; set; } = null!;
    public string SeverityLevel { get; set; } = "medium";
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
} 