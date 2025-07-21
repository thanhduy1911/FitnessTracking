namespace FoodService.DTOs.Category;

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