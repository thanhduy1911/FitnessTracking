using System.ComponentModel.DataAnnotations;

namespace FoodService.RequestHelpers.Nutrition;

/// <summary>
/// Recipe ingredient request with quantity for nutrition calculation
/// </summary>
public class RecipeIngredientRequest
{
    /// <summary>
    /// Food ID
    /// </summary>
    [Required(ErrorMessage = "Food ID là bắt buộc")]
    public Guid FoodId { get; set; }

    /// <summary>
    /// Quantity in grams
    /// </summary>
    [Range(0.1, 10000, ErrorMessage = "Quantity phải từ 0.1 đến 10000g")]
    public decimal QuantityGrams { get; set; }

    /// <summary>
    /// Optional ingredient name for display
    /// </summary>
    [StringLength(255, ErrorMessage = "Tên nguyên liệu không được vượt quá 255 ký tự")]
    public string? IngredientName { get; set; }

    /// <summary>
    /// Order in recipe
    /// </summary>
    [Range(1, 100, ErrorMessage = "Thứ tự phải từ 1 đến 100")]
    public int Order { get; set; }
}
