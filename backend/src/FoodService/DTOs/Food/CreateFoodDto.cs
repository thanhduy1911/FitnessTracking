using System.ComponentModel.DataAnnotations;
using FoodService.DTOs.Nutrition;

namespace FoodService.DTOs.Food;

/// <summary>
/// DTO for creating new food items
/// </summary>
public class CreateFoodDto
{
    [Required(ErrorMessage = "Tên thực phẩm là bắt buộc")]
    [StringLength(255, ErrorMessage = "Tên thực phẩm không được vượt quá 255 ký tự")]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(255, ErrorMessage = "Tên tiếng Anh không được vượt quá 255 ký tự")]
    public string NameEn { get; set; } = string.Empty;
    
    [StringLength(255, ErrorMessage = "Tên tiếng Việt không được vượt quá 255 ký tự")]
    public string NameVi { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    [StringLength(100, ErrorMessage = "Mã thực phẩm không được vượt quá 100 ký tự")]
    public string? FoodCode { get; set; }
    
    [StringLength(50, ErrorMessage = "Barcode không được vượt quá 50 ký tự")]
    public string Barcode { get; set; } = string.Empty;
    
    public Guid? CategoryId { get; set; }
    
    [Required(ErrorMessage = "Nguồn dữ liệu là bắt buộc")]
    [StringLength(50, ErrorMessage = "Nguồn dữ liệu không được vượt quá 50 ký tự")]
    public string DataSource { get; set; } = string.Empty;
    
    [StringLength(100, ErrorMessage = "External ID không được vượt quá 100 ký tự")]
    public string ExternalId { get; set; } = string.Empty;
    
    public string SourceUrl { get; set; } = string.Empty;
    
    // Serving size information
    [Range(0.1, 10000, ErrorMessage = "Khối lượng khẩu phần phải từ 0.1 đến 10000 grams")]
    public decimal? ServingSizeGrams { get; set; }
    
    [StringLength(100, ErrorMessage = "Mô tả khẩu phần không được vượt quá 100 ký tự")]
    public string ServingSizeDescription { get; set; } = string.Empty;
    
    [StringLength(100, ErrorMessage = "Mô tả khẩu phần tiếng Anh không được vượt quá 100 ký tự")]
    public string ServingSizeDescriptionEn { get; set; } = string.Empty;
    
    public List<AlternativeServingDto> AlternativeServingSizes { get; set; } = new();
    
    // Nutrition information
    public CreateNutritionFactsDto? NutritionFacts { get; set; }
    
    // Allergen information
    public List<Guid> AllergenIds { get; set; } = new();
    
    // Status
    public bool IsActive { get; set; } = true;
} 