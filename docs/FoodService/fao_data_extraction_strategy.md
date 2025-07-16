# FAO Vietnam Data Extraction & Import Strategy

## 1. Data Source Assessment

### FAO Vietnam Food Composition Table 2007 (VTN_FCT_2007)
- **Source**: FAO/INFOODS official database
- **Format**: PDF document (~300+ pages)
- **Scope**: ~1,000 Vietnamese food items
- **Nutrients**: 30+ nutritional components
- **Structure**: Categorized by food groups (cereals, vegetables, fruits, meat, fish, etc.)

### Data Quality Advantages:
- ✅ **Authoritative**: FAO official standard
- ✅ **Vietnam-specific**: Local foods and preparations
- ✅ **Comprehensive**: Wide nutrient coverage
- ✅ **Standardized**: International INFOODS format

## 2. Extraction Methods

### Method 1: Automated PDF Parsing (Primary)
```python
# Required libraries
import tabula
import pandas as pd
import PyPDF2
import pdfplumber

# Extract tables from PDF
def extract_fao_tables(pdf_path):
    # Method 1: Tabula for table extraction
    tables = tabula.read_pdf(pdf_path, pages="all", multiple_tables=True)
    
    # Method 2: pdfplumber for complex layouts
    with pdfplumber.open(pdf_path) as pdf:
        for page in pdf.pages:
            tables = page.extract_tables()
    
    return tables
```

### Method 2: Manual + OCR (Backup)
```
PDF → Adobe Acrobat → Export to Excel → Clean & Transform
```

### Method 3: Hybrid Approach (Recommended)
```
1. Auto-extract main tables (80% coverage)
2. Manual verification & correction
3. OCR for complex sections
4. Quality validation
```

## 3. Database Schema Mapping

### FAO Format → Database Tables

#### 3.1 Food Categories Mapping
```sql
-- food_categories table
FAO Group → category_id
"Cereals and cereal products" → UUID
"Vegetables and vegetable products" → UUID
"Fruits and fruit products" → UUID
"Meat and meat products" → UUID
```

#### 3.2 Foods Table Mapping
```sql
-- foods table
FAO Food Name → name_vi (Vietnamese name)
FAO Food Name (EN) → name_en (English name)
FAO Food Code → food_code
FAO Category → category_id
'fao_vn_2007' → data_source
FAO_ID → external_id
```

#### 3.3 Nutrition Facts Mapping
```sql
-- nutrition_facts table
FAO Nutrient Values → nutrition_facts columns

-- Common FAO nutrients:
Energy (kcal) → calories_kcal
Protein (g) → protein_g
Fat (g) → fat_g
Carbohydrate (g) → carbohydrate_g
Fiber (g) → fiber_g
Calcium (mg) → calcium_mg
Iron (mg) → iron_mg
Vitamin A (mcg) → vitamin_a_mcg
Vitamin C (mg) → vitamin_c_mg
```

## 4. Data Transformation Pipeline

### 4.1 Extraction Script
```python
class FAODataExtractor:
    def __init__(self, pdf_path):
        self.pdf_path = pdf_path
        self.raw_data = []
        
    def extract_tables(self):
        """Extract all tables from PDF"""
        tables = tabula.read_pdf(
            self.pdf_path, 
            pages="all", 
            multiple_tables=True,
            pandas_options={'header': 0}
        )
        return tables
    
    def clean_data(self, tables):
        """Clean and standardize extracted data"""
        cleaned_tables = []
        for table in tables:
            # Remove empty rows/columns
            table = table.dropna(how='all')
            
            # Standardize column names
            table.columns = self.standardize_columns(table.columns)
            
            # Data type conversion
            table = self.convert_data_types(table)
            
            cleaned_tables.append(table)
        
        return cleaned_tables
    
    def standardize_columns(self, columns):
        """Map FAO column names to database fields"""
        column_mapping = {
            'Food Name': 'name_vi',
            'Food Name (English)': 'name_en',
            'Food Code': 'food_code',
            'Energy (kcal)': 'calories_kcal',
            'Protein (g)': 'protein_g',
            'Fat (g)': 'fat_g',
            'Carbohydrate (g)': 'carbohydrate_g',
            # ... more mappings
        }
        return [column_mapping.get(col, col) for col in columns]
```

### 4.2 Data Validation
```python
def validate_fao_data(data):
    """Validate extracted data quality"""
    validation_rules = {
        'name_vi': 'required',
        'calories_kcal': 'numeric, >= 0',
        'protein_g': 'numeric, >= 0',
        'fat_g': 'numeric, >= 0',
        'carbohydrate_g': 'numeric, >= 0'
    }
    
    errors = []
    for rule in validation_rules:
        # Apply validation logic
        pass
    
    return errors
```

## 5. Database Import Strategy

### 5.1 Category Setup
```sql
-- Create food categories first
INSERT INTO food_categories (id, name_vi, name_en, description) VALUES
(gen_random_uuid(), 'Ngũ cốc và sản phẩm từ ngũ cốc', 'Cereals and cereal products', 'FAO Category 1'),
(gen_random_uuid(), 'Rau và sản phẩm từ rau', 'Vegetables and vegetable products', 'FAO Category 2'),
-- ... more categories
```

### 5.2 Food Items Import
```python
def import_foods_to_db(foods_data, db_connection):
    """Import foods to database"""
    for food_item in foods_data:
        # Insert into foods table
        food_id = insert_food(food_item)
        
        # Insert nutrition facts
        insert_nutrition_facts(food_id, food_item.nutrients)
        
        # Set verification status
        update_food_verification(food_id, 'approved', 'fao_import')
```

### 5.3 Batch Processing
```python
def process_fao_import_batch(batch_size=100):
    """Process import in batches to avoid memory issues"""
    with db_connection.begin() as trans:
        for i in range(0, len(foods_data), batch_size):
            batch = foods_data[i:i+batch_size]
            process_batch(batch)
            
            # Log progress
            print(f"Processed {i+len(batch)}/{len(foods_data)} items")
```

## 6. Quality Assurance

### 6.1 Data Verification Steps
```python
def verify_import_quality():
    """Verify data quality after import"""
    checks = [
        'check_duplicate_foods()',
        'validate_nutrition_ranges()',
        'verify_category_assignments()',
        'check_missing_nutrients()',
        'validate_vietnamese_names()'
    ]
    
    for check in checks:
        result = eval(check)
        if not result.passed:
            print(f"Quality check failed: {check}")
```

### 6.2 Data Completeness Report
```sql
-- Generate completeness report
SELECT 
    fc.name_vi as category,
    COUNT(f.id) as total_foods,
    AVG(CASE WHEN nf.calories_kcal IS NOT NULL THEN 1 ELSE 0 END) * 100 as calories_completeness,
    AVG(CASE WHEN nf.protein_g IS NOT NULL THEN 1 ELSE 0 END) * 100 as protein_completeness,
    AVG(CASE WHEN nf.vitamin_c_mg IS NOT NULL THEN 1 ELSE 0 END) * 100 as vitamin_c_completeness
FROM foods f
JOIN food_categories fc ON f.category_id = fc.id
LEFT JOIN nutrition_facts nf ON f.id = nf.food_id
WHERE f.data_source = 'fao_vn_2007'
GROUP BY fc.name_vi;
```

## 7. Seed Data Strategy

### 7.1 Priority Import Order
```
1. Food categories (20-30 categories)
2. Common staples (rice, noodles, bread) - 50 items
3. Vegetables - 200 items
4. Fruits - 150 items
5. Meat & fish - 200 items
6. Processed foods - 300 items
```

### 7.2 Seed Data Script
```python
def create_seed_data():
    """Create initial seed data for development"""
    # Priority foods for MVP
    priority_foods = [
        'Gạo tẻ',
        'Bánh mì',
        'Thịt heo',
        'Cá tra',
        'Cà chua',
        'Cải ngồng',
        'Chuối',
        'Cam',
        # ... 100 most common foods
    ]
    
    return extract_priority_foods(priority_foods)
```

## 8. Implementation Timeline

### Phase 1: Setup & Testing (Week 1-2)
- [ ] Setup extraction environment
- [ ] Test PDF parsing tools
- [ ] Create sample extraction scripts
- [ ] Test database import pipeline

### Phase 2: Full Extraction (Week 3-4)
- [ ] Extract all FAO tables
- [ ] Clean and validate data
- [ ] Create import scripts
- [ ] Test data quality

### Phase 3: Database Import (Week 5-6)
- [ ] Import food categories
- [ ] Import food items
- [ ] Import nutrition facts
- [ ] Verify data integrity

### Phase 4: Quality Assurance (Week 7-8)
- [ ] Run quality checks
- [ ] Fix data issues
- [ ] Create documentation
- [ ] Prepare for API integration

## 9. Technical Considerations

### 9.1 Challenges & Solutions
```
Challenge: PDF table extraction accuracy
Solution: Use multiple tools + manual verification

Challenge: Vietnamese character encoding
Solution: Ensure UTF-8 encoding throughout

Challenge: Inconsistent nutrient units
Solution: Standardize all units to /100g

Challenge: Missing or incomplete data
Solution: Mark as NULL, don't estimate
```

### 9.2 Performance Optimization
```python
# Use bulk inserts for better performance
def bulk_insert_foods(foods_data):
    """Bulk insert for better performance"""
    foods_sql = """
    INSERT INTO foods (id, name_vi, name_en, food_code, category_id, data_source)
    VALUES %s
    """
    
    nutrition_sql = """
    INSERT INTO nutrition_facts (food_id, calories_kcal, protein_g, fat_g, carbohydrate_g)
    VALUES %s
    """
    
    # Execute bulk inserts
    execute_values(cursor, foods_sql, foods_values)
    execute_values(cursor, nutrition_sql, nutrition_values)
```

## 10. Next Steps

### Immediate Actions:
1. **Download FAO Vietnam FCT 2007 PDF**
2. **Test extraction tools** on sample pages
3. **Create database migration** for FAO data structure
4. **Build extraction pipeline** prototype

### Long-term Goals:
1. **Automate updates** when new FAO data released
2. **Create data reconciliation** with other sources
3. **Build data quality dashboard**
4. **Establish data governance** policies

---

**Success Metrics:**
- ✅ 1000+ Vietnamese foods imported
- ✅ 95%+ data accuracy achieved
- ✅ All major nutrient components covered
- ✅ Ready for API integration

**Estimated Timeline:** 8 weeks
**Resource Requirements:** 1 developer, PDF extraction tools, database access 