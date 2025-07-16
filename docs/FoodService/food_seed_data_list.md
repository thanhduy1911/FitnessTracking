# Vietnamese Food Seed Data List (100 Items)

## 1. STAPLE FOODS (Thực phẩm chủ yếu) - 20 items

### Rice & Rice Products
1. **Cơm tẻ** - Steamed white rice
2. **Cơm nếp** - Steamed glutinous rice
3. **Cháo trắng** - Plain rice porridge
4. **Bánh chưng** - Square sticky rice cake
5. **Bánh tét** - Cylindrical sticky rice cake
6. **Bánh tráng** - Rice paper
7. **Bánh phở** - Fresh rice noodles
8. **Bún** - Rice vermicelli
9. **Miến** - Mung bean noodles
10. **Bánh canh** - Thick rice noodles

### Bread & Wheat Products
11. **Bánh mì** - Vietnamese baguette
12. **Mì tôm** - Instant noodles
13. **Mì sợi** - Wheat noodles
14. **Bánh bao** - Steamed buns
15. **Bánh quy** - Crackers

### Root Vegetables
16. **Khoai lang** - Sweet potato
17. **Khoai tây** - Potato
18. **Khoai môn** - Taro
19. **Sắn** - Cassava
20. **Củ dong** - Jicama

## 2. VEGETABLES (Rau củ) - 25 items

### Leafy Greens
21. **Rau muống** - Water spinach
22. **Cải ngồng** - Bok choy
23. **Cải bẹ xanh** - Chinese cabbage
24. **Rau cải** - Mustard greens
25. **Rau ngót** - Amaranth leaves
26. **Rau dền** - Amaranth
27. **Rau lang** - Sweet potato leaves
28. **Xà lách** - Lettuce

### Common Vegetables
29. **Cà chua** - Tomato
30. **Cà tím** - Eggplant
31. **Bí đao** - Winter melon
32. **Mướp** - Luffa
33. **Khổ qua** - Bitter gourd
34. **Đậu cove** - Yard-long beans
35. **Đậu bắp** - Okra
36. **Hành lá** - Scallions
37. **Hành tây** - Onion
38. **Tỏi** - Garlic
39. **Gừng** - Ginger
40. **Ớt** - Chili pepper
41. **Củ cải trắng** - Daikon radish
42. **Cà rốt** - Carrot
43. **Giá đỗ** - Bean sprouts
44. **Nấm hương** - Shiitake mushroom
45. **Nấm rơm** - Straw mushroom

## 3. FRUITS (Trái cây) - 15 items

### Tropical Fruits
46. **Chuối** - Banana
47. **Cam** - Orange
48. **Quýt** - Mandarin
49. **Đu đủ** - Papaya
50. **Xoài** - Mango
51. **Dứa** - Pineapple
52. **Dưa hấu** - Watermelon
53. **Dưa lưới** - Cantaloupe
54. **Nho** - Grapes
55. **Táo** - Apple
56. **Lê** - Pear
57. **Chôm chôm** - Rambutan
58. **Nhãn** - Longan
59. **Vải** - Lychee
60. **Thanh long** - Dragon fruit

## 4. PROTEIN SOURCES (Thực phẩm đạm) - 25 items

### Meat
61. **Thịt heo** - Pork
62. **Thịt bò** - Beef
63. **Thịt gà** - Chicken
64. **Thịt vịt** - Duck
65. **Thịt dê** - Goat
66. **Lòng heo** - Pork organs
67. **Giò chả** - Vietnamese pork sausage

### Fish & Seafood
68. **Cá tra** - Pangasius
69. **Cá rô phi** - Tilapia
70. **Cá trắm** - Common carp
71. **Cá thu** - Tuna
72. **Cá hồi** - Salmon
73. **Tôm** - Shrimp
74. **Cua** - Crab
75. **Nghêu** - Clams
76. **Cá mắm** - Anchovy sauce fish

### Eggs & Dairy
77. **Trứng gà** - Chicken eggs
78. **Trứng vịt** - Duck eggs
79. **Sữa tươi** - Fresh milk
80. **Sữa chua** - Yogurt
81. **Phô mai** - Cheese

### Legumes & Nuts
82. **Đậu phộng** - Peanuts
83. **Đậu xanh** - Mung beans
84. **Đậu đỏ** - Red beans
85. **Đậu hũ** - Tofu
86. **Tương** - Soybean paste

## 5. CONDIMENTS & SEASONINGS (Gia vị) - 10 items

87. **Nước mắm** - Fish sauce
88. **Tương ớt** - Chili sauce
89. **Dầu ăn** - Cooking oil
90. **Muối** - Salt
91. **Đường** - Sugar
92. **Giấm** - Vinegar
93. **Hạt tiêu** - Black pepper
94. **Bột ngọt** - MSG
95. **Nước tương** - Soy sauce
96. **Mắm tôm** - Shrimp paste

## 6. BEVERAGES (Đồ uống) - 5 items

97. **Nước lọc** - Filtered water
98. **Trà** - Tea
99. **Cà phê** - Coffee
100. **Nước dừa** - Coconut water

---

## Nutrition Data Structure for Each Item

```json
{
  "id": "uuid",
  "name_vi": "Cơm tẻ",
  "name_en": "Steamed white rice",
  "food_code": "VN001",
  "category": "Staple Foods",
  "category_id": "uuid",
  "data_source": "manual_seed",
  "serving_size_g": 100,
  "serving_description": "1 bowl",
  "nutrition_facts": {
    "calories_kcal": 130,
    "protein_g": 2.7,
    "fat_g": 0.3,
    "carbohydrate_g": 28.0,
    "fiber_g": 0.4,
    "sugar_g": 0.1,
    "sodium_mg": 5,
    "calcium_mg": 28,
    "iron_mg": 0.8,
    "vitamin_c_mg": 0,
    "vitamin_a_mcg": 0
  },
  "is_verified": true,
  "verification_status": "approved"
}
```

## Category Distribution

- **Staple Foods**: 20 items (20%)
- **Vegetables**: 25 items (25%)
- **Fruits**: 15 items (15%)
- **Protein Sources**: 25 items (25%)
- **Condiments & Seasonings**: 10 items (10%)
- **Beverages**: 5 items (5%)

## Priority Implementation Order

### Phase 1: Core Essentials (25 items)
- Top 10 staple foods
- Top 10 vegetables
- Top 5 fruits

### Phase 2: Protein & Variety (50 items)
- All protein sources
- Remaining vegetables & fruits

### Phase 3: Complete Set (100 items)
- Condiments & seasonings
- Beverages
- Specialty items

## Data Sources for Nutrition Values

1. **FAO Vietnam FCT 2007** - Primary source
2. **USDA Food Data Central** - Secondary for missing items
3. **Vietnam National Institute of Nutrition** - Local data
4. **Manual estimation** - For processed/prepared foods

## Quality Assurance

- ✅ All items are commonly consumed in Vietnam
- ✅ Covers major food groups
- ✅ Balanced representation
- ✅ Suitable for fitness tracking
- ✅ Ready for API integration 