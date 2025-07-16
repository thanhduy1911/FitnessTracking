using System.ComponentModel.DataAnnotations;

namespace FoodService.Entities;

public class ServingSize
{
    public decimal Grams { get; set; }
    
    [StringLength(100)]
    public string Description { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string DescriptionEn { get; set; } = string.Empty;
    
    public string? Category { get; set; }  // "standard", "small", "large", "extra_large"
    
    public bool IsDefault { get; set; }
}

// DTO for serving size calculations
public class ServingSizeCalculation
{
    public decimal ServingGrams { get; set; }
    public string ServingDescription { get; set; } = string.Empty;
    public NutritionFacts NutritionPerServing { get; set; } = new();
    public decimal MultiplierFrom100g { get; set; }
}

// Helper class for common Vietnamese serving sizes
public static class VietnameseServingSizes
{
    public static Dictionary<string, decimal> CommonServings = new()
    {
        // Rice & Noodles
        ["cơm_bát"] = 150,          // 1 bát cơm
        ["phở_tô"] = 350,           // 1 tô phở
        ["bún_bát"] = 200,          // 1 bát bún
        ["bánh_mì"] = 80,           // 1 ổ bánh mì
        
        // Fruits
        ["chuối_quả"] = 120,        // 1 quả chuối
        ["cam_quả"] = 150,          // 1 quả cam
        ["táo_quả"] = 180,          // 1 quả táo
        ["xoài_quả"] = 200,         // 1 quả xoài
        
        // Vegetables
        ["rau_dĩa"] = 100,          // 1 dĩa rau
        ["cà_chua_quả"] = 120,      // 1 quả cà chua
        
        // Beverages
        ["nước_ly"] = 250,          // 1 ly nước
        ["trà_ly"] = 200,           // 1 ly trà
        ["cà_phê_ly"] = 180,        // 1 ly cà phê
        
        // Meat & Protein
        ["thịt_miếng"] = 100,       // 1 miếng thịt
        ["cá_khúc"] = 150,          // 1 khúc cá
        ["trứng_quả"] = 60,         // 1 quả trứng
        
        // Snacks
        ["bánh_cái"] = 50,          // 1 cái bánh
        ["kẹo_viên"] = 5,           // 1 viên kẹo
    };
    
    public static decimal GetServingSize(string foodType, string servingKey)
    {
        return CommonServings.ContainsKey(servingKey) ? CommonServings[servingKey] : 100;
    }
} 