# Food API Research & Strategy - Fitness Tracking Platform

## 1. API Research Results

### 1.1 USDA Food Data Central API (Recommended Primary)

**Overview**: Official US Government food database với comprehensive nutrition data

**Pros**:
- **Free**: Hoàn toàn miễn phí
- **Comprehensive**: 400,000+ food items với detailed nutrition info
- **Reliable**: Government-maintained database
- **Rate limits**: 1,000 requests/hour (adequate for most apps)
- **Multiple formats**: JSON, XML support
- **Rich data**: Ingredients, nutrition facts, allergens, brands

**Cons**:
- **US-focused**: Limited Vietnamese food coverage
- **English only**: Không có Vietnamese food names
- **Complex structure**: Requires mapping to simpler format

**API Endpoints**:
```
GET /api/v1/food/{fdc_id}
GET /api/v1/foods/search?query={query}
GET /api/v1/foods/list
```

**Example Response**:
```json
{
  "fdc_id": 167512,
  "description": "Chicken breast, boneless, skinless, raw",
  "food_nutrients": [
    {
      "nutrient": {"name": "Protein", "unit_name": "g"},
      "amount": 23.09
    }
  ]
}
```

### 1.2 OpenFoodFacts API (Recommended Secondary)

**Overview**: Community-driven global food database, perfect for barcode scanning

**Pros**:
- **Free**: Completely free and open source
- **Barcode support**: Excellent for packaged foods
- **Global coverage**: Products từ worldwide including Vietnam
- **Community-driven**: Users can contribute data
- **Real products**: Actual packaged foods với barcodes
- **Vietnamese products**: Some Vietnamese packaged foods available

**Cons**:
- **Variable quality**: Community data có thể không consistent
- **Limited fresh foods**: Focus on packaged products
- **Rate limits**: 10 requests/min for search (quite low)

**API Endpoints**:
```
GET /api/v0/product/{barcode}.json
GET /cgi/search.pl?search_terms={query}
```

**Example Response**:
```json
{
  "product": {
    "product_name": "Trà Sữa Ô Long",
    "brands": "Phúc Long",
    "nutriments": {
      "energy-kcal_100g": 45,
      "proteins_100g": 0.8,
      "carbohydrates_100g": 10.5
    }
  }
}
```

### 1.3 Vietnamese Food Data Sources

**Current Status**: Limited Vietnamese-specific APIs available

**Available Sources**:
1. **iNUT API**: Vietnamese nutrition project (limited scope)
2. **Government databases**: PDF/Excel files, không có API
3. **Academic research**: FAO Vietnam Food Composition Table (PDF only)
4. **Community projects**: Small-scale GitHub projects

**Strategy**: Hybrid approach với manual data entry

## 2. Recommended Strategy

### 2.1 Hybrid Data Approach

**Phase 1: Foundation (MVP)**
```
Primary: USDA Food Data Central
- Basic international foods
- Common ingredients
- Nutrition calculations

Secondary: OpenFoodFacts
- Packaged foods
- Barcode scanning
- Popular Vietnamese packaged products
```

**Phase 2: Vietnamese Data (Enhanced)**
```
Manual curation:
- Common Vietnamese dishes
- Local ingredients
- Regional specialties
- User-contributed data với verification
```

**Phase 3: Community (Advanced)**
```
User-generated content:
- Recipe submissions
- Nutrition verification
- Photo submissions
- Community voting system
```

### 2.2 Database Design Strategy

**Foods Table Structure**:
```sql
CREATE TABLE foods (
    id UUID PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    name_en VARCHAR(255),
    name_vi VARCHAR(255),
    food_code VARCHAR(100),
    data_source VARCHAR(50), -- 'usda', 'openfoodfacts', 'user', 'manual'
    external_id VARCHAR(100),
    category_id UUID,
    serving_size_g DECIMAL(8,2),
    verified BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP,
    updated_at TIMESTAMP
);
```

**Nutrition Table Structure**:
```sql
CREATE TABLE nutrition_facts (
    id UUID PRIMARY KEY,
    food_id UUID REFERENCES foods(id),
    calories_per_100g DECIMAL(8,2),
    protein_g DECIMAL(8,2),
    carbs_g DECIMAL(8,2),
    fat_g DECIMAL(8,2),
    fiber_g DECIMAL(8,2),
    sugar_g DECIMAL(8,2),
    sodium_mg DECIMAL(8,2),
    -- Additional micronutrients
    vitamin_a_mcg DECIMAL(8,2),
    vitamin_c_mg DECIMAL(8,2),
    calcium_mg DECIMAL(8,2),
    iron_mg DECIMAL(8,2)
);
```

### 2.3 API Integration Plan

**USDA Integration**:
```csharp
public class UsdaFoodService
{
    private readonly string _apiKey;
    private readonly string _baseUrl = "https://api.nal.usda.gov/fdc/v1";
    
    public async Task<FoodDto> SearchFoodAsync(string query)
    {
        var url = $"{_baseUrl}/foods/search?query={query}&api_key={_apiKey}";
        // Implementation
    }
}
```

**OpenFoodFacts Integration**:
```csharp
public class OpenFoodFactsService
{
    private readonly string _baseUrl = "https://world.openfoodfacts.org/api/v0";
    
    public async Task<ProductDto> GetProductByBarcodeAsync(string barcode)
    {
        var url = $"{_baseUrl}/product/{barcode}.json";
        // Implementation
    }
}
```

## 3. Vietnamese Food Data Strategy

### 3.1 Manual Curation Priority

**High Priority Foods** (500+ items):
```
Common dishes:
- Phở (various types)
- Cơm tấm
- Bánh mì
- Bún bò Huế
- Gỏi cuốn
- Bánh xèo

Ingredients:
- Nước mắm
- Rau muống
- Cải thìa
- Thịt ba chỉ
- Cá tra
- Tôm tít
```

**Medium Priority** (1000+ items):
```
Regional specialties:
- Northern dishes
- Central dishes  
- Southern dishes
- Street food
- Desserts
```

**Low Priority** (2000+ items):
```
Less common:
- Seasonal dishes
- Festival foods
- Regional variants
- Modern fusion
```

### 3.2 Data Collection Methods

**Method 1: Government Data**
- Download Vietnam Food Composition Tables
- Convert PDF/Excel to database format
- Validate against other sources

**Method 2: Restaurant Partnerships**
- Partner với Vietnamese restaurants
- Get nutrition info for common dishes
- Standardize serving sizes

**Method 3: Community Contributions**
- Allow users to submit food data
- Verification process
- Reputation system

**Method 4: Nutritionist Collaboration**
- Work với Vietnamese nutritionists
- Professional verification
- Cultural context

### 3.3 Data Quality Assurance

**Verification Process**:
1. **Multiple sources**: Cross-reference với 2+ sources
2. **Expert review**: Nutritionist validation
3. **Community voting**: User verification system
4. **Audit trail**: Track data changes và sources

**Quality Metrics**:
- Source reliability score
- Community confidence rating
- Professional verification status
- Usage frequency weighting

## 4. Implementation Plan

### 4.1 Phase 1: API Integration (Month 1-2)

**Week 1-2: USDA Integration**
- Setup API authentication
- Implement search functionality
- Create data mapping layer
- Test với common foods

**Week 3-4: OpenFoodFacts Integration**
- Implement barcode scanning
- Product lookup functionality
- Error handling for missing data
- Cache frequently accessed items

### 4.2 Phase 2: Vietnamese Data (Month 3-4)

**Week 1-2: Data Collection**
- Collect government food composition data
- Manual entry of 100 most common Vietnamese foods
- Setup user contribution system

**Week 3-4: Verification System**
- Build admin approval workflow
- Implement community voting
- Create data quality metrics

### 4.3 Phase 3: Advanced Features (Month 5-6)

**Week 1-2: Photo Recognition**
- Integrate ML.NET for food photo recognition
- Train với Vietnamese food images
- Implement confidence scoring

**Week 3-4: Recipe Analysis**
- Recipe ingredient parsing
- Automatic nutrition calculation
- Serving size estimation

## 5. Cost Analysis

### 5.1 API Costs (Monthly)

**USDA Food Data Central**: $0 (Free)
**OpenFoodFacts**: $0 (Free)
**Photo Recognition**: $10-30 (Cloud services)
**Data Storage**: $5-15 (PostgreSQL hosting)

**Total Monthly Cost**: $15-45

### 5.2 Development Costs

**Initial Setup**: 40-60 hours
**Vietnamese Data Curation**: 80-120 hours
**Ongoing Maintenance**: 10-20 hours/month

## 6. Risk Mitigation

### 6.1 Technical Risks

**API Rate Limits**:
- Solution: Implement caching layer
- Fallback: Local database với periodic sync

**Data Quality Issues**:
- Solution: Multi-source verification
- Fallback: Community reporting system

**Vietnamese Data Gaps**:
- Solution: Gradual manual curation
- Fallback: Generic ingredient substitution

### 6.2 Business Risks

**User Adoption**:
- Solution: Focus on accuracy và Vietnamese coverage
- Fallback: Gamification để encourage contributions

**Competition**:
- Solution: Open source approach
- Fallback: Unique Vietnamese focus

## 7. Success Metrics

### 7.1 Data Coverage

**Target Metrics**:
- 1,000+ Vietnamese foods (Month 6)
- 10,000+ international foods (Month 3)
- 95% accuracy rate (Month 12)
- 80% user satisfaction (Month 6)

### 7.2 User Engagement

**Target Metrics**:
- 500+ monthly active users (Month 6)
- 100+ user contributions/month (Month 9)
- 90% successful food lookups (Month 3)

---

**Recommendation**: Start với USDA + OpenFoodFacts APIs, gradually build Vietnamese database through community contributions và manual curation. Focus on accuracy over coverage initially. 