namespace FoodService.DTOs.Food;

/// <summary>
/// Lightweight DTO for food listing - optimized for mobile performance
/// </summary>
public class FoodListDto
{
    public Guid Id { get; set; }
    public string NameVi { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public decimal? ServingSizeGrams { get; set; }
    public string ServingSizeDescription { get; set; } = string.Empty;
    public decimal? CaloriesKcal { get; set; }
    public decimal? ProteinG { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
    public string DataSource { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
} 