using System.ComponentModel.DataAnnotations;
using FoodService.DTOs.Nutrition;

namespace FoodService.DTOs.Food;

/// <summary>
/// DTO for updating existing food items
/// </summary>
public class UpdateFoodDto
{
    [StringLength(255, ErrorMessage = "Tên thực phẩm không được vượt quá 255 ký tự")]
    public string? Name { get; set; }
    
    [StringLength(255, ErrorMessage = "Tên tiếng Anh không được vượt quá 255 ký tự")]
    public string? NameEn { get; set; }
    
    [StringLength(255, ErrorMessage = "Tên tiếng Việt không được vượt quá 255 ký tự")]
    public string? NameVi { get; set; }
    
    public string? Description { get; set; }
    
    [StringLength(100, ErrorMessage = "Mã thực phẩm không được vượt quá 100 ký tự")]
    public string? FoodCode { get; set; }
    
    [StringLength(50, ErrorMessage = "Barcode không được vượt quá 50 ký tự")]
    public string? Barcode { get; set; }
    
    public Guid? CategoryId { get; set; }
    
    [StringLength(50, ErrorMessage = "Nguồn dữ liệu không được vượt quá 50 ký tự")]
    public string? DataSource { get; set; }
    
    [StringLength(100, ErrorMessage = "External ID không được vượt quá 100 ký tự")]
    public string? ExternalId { get; set; }
    
    public string? SourceUrl { get; set; }
    
    // Serving size information
    [Range(0.1, 10000, ErrorMessage = "Khối lượng khẩu phần phải từ 0.1 đến 10000 grams")]
    public decimal? ServingSizeGrams { get; set; }
    
    [StringLength(100, ErrorMessage = "Mô tả khẩu phần không được vượt quá 100 ký tự")]
    public string? ServingSizeDescription { get; set; }
    
    [StringLength(100, ErrorMessage = "Mô tả khẩu phần tiếng Anh không được vượt quá 100 ký tự")]
    public string? ServingSizeDescriptionEn { get; set; }
    
    public List<AlternativeServingDto>? AlternativeServingSizes { get; set; }
    
    // Nutrition information
    public UpdateNutritionFactsDto? NutritionFacts { get; set; }
    
    // Allergen information
    public List<Guid>? AllergenIds { get; set; }
    
    // Status
    public bool? IsActive { get; set; }
} 