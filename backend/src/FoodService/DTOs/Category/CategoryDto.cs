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