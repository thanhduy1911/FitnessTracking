# Food Database Design - Fitness Tracking Platform

## 1. Database Overview

### 1.1 Purpose
FoodService database quản lý:
- Food items và nutrition information
- User-contributed recipes
- Barcode scanning data
- Meal planning và tracking
- Food categories và allergens

### 1.2 Technology Stack
- **Database**: PostgreSQL 15+
- **ORM**: Entity Framework Core 8
- **Caching**: Redis
- **Search**: Full-text search + future Elasticsearch integration

## 2. Core Tables

### 2.1 Food Categories

```sql
CREATE TABLE food_categories (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    name_en VARCHAR(255),
    name_vi VARCHAR(255),
    description TEXT,
    parent_id UUID REFERENCES food_categories(id),
    sort_order INTEGER DEFAULT 0,
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_food_categories_parent ON food_categories(parent_id);
CREATE INDEX idx_food_categories_name ON food_categories(name);
```

### 2.2 Foods (Main Table)

```sql
CREATE TABLE foods (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    name_en VARCHAR(255),
    name_vi VARCHAR(255),
    description TEXT,
    food_code VARCHAR(100), -- External reference (USDA, OpenFoodFacts)
    barcode VARCHAR(50),
    category_id UUID REFERENCES food_categories(id),
    
    -- Data source tracking
    data_source VARCHAR(50) NOT NULL, -- 'usda', 'openfoodfacts', 'user', 'manual'
    external_id VARCHAR(100),
    source_url TEXT,
    
    -- Serving information
    serving_size_g DECIMAL(8,2),
    serving_size_description VARCHAR(255), -- "1 cup", "1 medium apple"
    
    -- Status và verification
    is_verified BOOLEAN DEFAULT FALSE,
    verification_status VARCHAR(20) DEFAULT 'pending', -- 'pending', 'approved', 'rejected'
    verified_by UUID, -- User ID who verified
    verified_at TIMESTAMP WITH TIME ZONE,
    
    -- Metadata
    is_active BOOLEAN DEFAULT TRUE,
    created_by UUID, -- User ID who created
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    
    -- Search optimization
    search_vector TSVECTOR
);

-- Indexes
CREATE INDEX idx_foods_category ON foods(category_id);
CREATE INDEX idx_foods_barcode ON foods(barcode);
CREATE INDEX idx_foods_data_source ON foods(data_source);
CREATE INDEX idx_foods_verification ON foods(verification_status);
CREATE INDEX idx_foods_search ON foods USING GIN(search_vector);
CREATE UNIQUE INDEX idx_foods_barcode_unique ON foods(barcode) WHERE barcode IS NOT NULL;

-- Full-text search trigger
CREATE OR REPLACE FUNCTION update_foods_search_vector() RETURNS TRIGGER AS $$
BEGIN
    NEW.search_vector := to_tsvector('english', 
        COALESCE(NEW.name, '') || ' ' || 
        COALESCE(NEW.name_en, '') || ' ' || 
        COALESCE(NEW.name_vi, '') || ' ' || 
        COALESCE(NEW.description, '')
    );
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_foods_search_vector
    BEFORE INSERT OR UPDATE ON foods
    FOR EACH ROW EXECUTE FUNCTION update_foods_search_vector();
```

### 2.3 Nutrition Facts

```sql
CREATE TABLE nutrition_facts (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    food_id UUID NOT NULL REFERENCES foods(id) ON DELETE CASCADE,
    
    -- Basic macronutrients (per 100g)
    calories_per_100g DECIMAL(8,2),
    protein_g DECIMAL(8,2),
    carbohydrates_g DECIMAL(8,2),
    fat_g DECIMAL(8,2),
    fiber_g DECIMAL(8,2),
    sugar_g DECIMAL(8,2),
    sodium_mg DECIMAL(8,2),
    
    -- Detailed fats
    saturated_fat_g DECIMAL(8,2),
    monounsaturated_fat_g DECIMAL(8,2),
    polyunsaturated_fat_g DECIMAL(8,2),
    trans_fat_g DECIMAL(8,2),
    cholesterol_mg DECIMAL(8,2),
    
    -- Vitamins
    vitamin_a_mcg DECIMAL(8,2),
    vitamin_c_mg DECIMAL(8,2),
    vitamin_d_mcg DECIMAL(8,2),
    vitamin_e_mg DECIMAL(8,2),
    vitamin_k_mcg DECIMAL(8,2),
    thiamine_mg DECIMAL(8,2),
    riboflavin_mg DECIMAL(8,2),
    niacin_mg DECIMAL(8,2),
    vitamin_b6_mg DECIMAL(8,2),
    folate_mcg DECIMAL(8,2),
    vitamin_b12_mcg DECIMAL(8,2),
    
    -- Minerals
    calcium_mg DECIMAL(8,2),
    iron_mg DECIMAL(8,2),
    magnesium_mg DECIMAL(8,2),
    phosphorus_mg DECIMAL(8,2),
    potassium_mg DECIMAL(8,2),
    zinc_mg DECIMAL(8,2),
    copper_mg DECIMAL(8,2),
    manganese_mg DECIMAL(8,2),
    selenium_mcg DECIMAL(8,2),
    
    -- Metadata
    data_source VARCHAR(50),
    confidence_score DECIMAL(3,2), -- 0.00 to 1.00
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_nutrition_facts_food ON nutrition_facts(food_id);
CREATE UNIQUE INDEX idx_nutrition_facts_food_unique ON nutrition_facts(food_id);
```

### 2.4 Allergens

```sql
CREATE TABLE allergens (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(100) NOT NULL,
    name_en VARCHAR(100),
    name_vi VARCHAR(100),
    description TEXT,
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE food_allergens (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    food_id UUID NOT NULL REFERENCES foods(id) ON DELETE CASCADE,
    allergen_id UUID NOT NULL REFERENCES allergens(id) ON DELETE CASCADE,
    severity VARCHAR(20) DEFAULT 'contains', -- 'contains', 'may_contain', 'traces'
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    
    UNIQUE(food_id, allergen_id)
);

CREATE INDEX idx_food_allergens_food ON food_allergens(food_id);
CREATE INDEX idx_food_allergens_allergen ON food_allergens(allergen_id);
```

### 2.5 Ingredients

```sql
CREATE TABLE ingredients (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    name_en VARCHAR(255),
    name_vi VARCHAR(255),
    description TEXT,
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE food_ingredients (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    food_id UUID NOT NULL REFERENCES foods(id) ON DELETE CASCADE,
    ingredient_id UUID NOT NULL REFERENCES ingredients(id) ON DELETE CASCADE,
    order_index INTEGER, -- Order in ingredient list
    percentage DECIMAL(5,2), -- Optional percentage
    is_main_ingredient BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_food_ingredients_food ON food_ingredients(food_id);
CREATE INDEX idx_food_ingredients_ingredient ON food_ingredients(ingredient_id);
CREATE INDEX idx_food_ingredients_order ON food_ingredients(food_id, order_index);
```

## 3. Recipe Management

### 3.1 Recipes

```sql
CREATE TABLE recipes (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    name_en VARCHAR(255),
    name_vi VARCHAR(255),
    description TEXT,
    instructions TEXT,
    
    -- Recipe metadata
    prep_time_minutes INTEGER,
    cook_time_minutes INTEGER,
    total_time_minutes INTEGER,
    servings INTEGER,
    difficulty_level VARCHAR(20), -- 'easy', 'medium', 'hard'
    
    -- Recipe source
    source_url TEXT,
    source_name VARCHAR(255),
    created_by UUID, -- User ID
    
    -- Status
    is_public BOOLEAN DEFAULT TRUE,
    is_verified BOOLEAN DEFAULT FALSE,
    verification_status VARCHAR(20) DEFAULT 'pending',
    
    -- Metadata
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_recipes_created_by ON recipes(created_by);
CREATE INDEX idx_recipes_verification ON recipes(verification_status);
CREATE INDEX idx_recipes_public ON recipes(is_public);
```

### 3.2 Recipe Ingredients

```sql
CREATE TABLE recipe_ingredients (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    recipe_id UUID NOT NULL REFERENCES recipes(id) ON DELETE CASCADE,
    food_id UUID NOT NULL REFERENCES foods(id),
    
    -- Quantity information
    quantity DECIMAL(10,2) NOT NULL,
    unit VARCHAR(50) NOT NULL, -- 'g', 'kg', 'ml', 'l', 'cup', 'tbsp', 'tsp', 'piece'
    quantity_in_grams DECIMAL(10,2), -- Normalized weight
    
    -- Display information
    preparation_notes TEXT, -- "chopped", "diced", "cooked"
    order_index INTEGER,
    
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_recipe_ingredients_recipe ON recipe_ingredients(recipe_id);
CREATE INDEX idx_recipe_ingredients_food ON recipe_ingredients(food_id);
CREATE INDEX idx_recipe_ingredients_order ON recipe_ingredients(recipe_id, order_index);
```

### 3.3 Recipe Categories

```sql
CREATE TABLE recipe_categories (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    name_en VARCHAR(255),
    name_vi VARCHAR(255),
    description TEXT,
    parent_id UUID REFERENCES recipe_categories(id),
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE recipe_category_mappings (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    recipe_id UUID NOT NULL REFERENCES recipes(id) ON DELETE CASCADE,
    category_id UUID NOT NULL REFERENCES recipe_categories(id) ON DELETE CASCADE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    
    UNIQUE(recipe_id, category_id)
);
```

## 4. User Interactions

### 4.1 User Food Contributions

```sql
CREATE TABLE user_food_contributions (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL,
    food_id UUID NOT NULL REFERENCES foods(id),
    contribution_type VARCHAR(50) NOT NULL, -- 'created', 'updated', 'verified', 'flagged'
    old_data JSONB,
    new_data JSONB,
    notes TEXT,
    status VARCHAR(20) DEFAULT 'pending', -- 'pending', 'approved', 'rejected'
    reviewed_by UUID, -- Admin user ID
    reviewed_at TIMESTAMP WITH TIME ZONE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_user_contributions_user ON user_food_contributions(user_id);
CREATE INDEX idx_user_contributions_food ON user_food_contributions(food_id);
CREATE INDEX idx_user_contributions_status ON user_food_contributions(status);
```

### 4.2 Food Ratings & Reviews

```sql
CREATE TABLE food_ratings (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    food_id UUID NOT NULL REFERENCES foods(id) ON DELETE CASCADE,
    user_id UUID NOT NULL,
    rating INTEGER NOT NULL CHECK (rating >= 1 AND rating <= 5),
    review TEXT,
    is_verified_purchase BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    
    UNIQUE(food_id, user_id)
);

CREATE INDEX idx_food_ratings_food ON food_ratings(food_id);
CREATE INDEX idx_food_ratings_user ON food_ratings(user_id);
CREATE INDEX idx_food_ratings_rating ON food_ratings(rating);
```

### 4.3 User Favorites

```sql
CREATE TABLE user_favorite_foods (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL,
    food_id UUID NOT NULL REFERENCES foods(id) ON DELETE CASCADE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    
    UNIQUE(user_id, food_id)
);

CREATE INDEX idx_user_favorites_user ON user_favorite_foods(user_id);
CREATE INDEX idx_user_favorites_food ON user_favorite_foods(food_id);
```

## 5. Meal Planning & Tracking

### 5.1 Meal Plans

```sql
CREATE TABLE meal_plans (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_meal_plans_user ON meal_plans(user_id);
CREATE INDEX idx_meal_plans_dates ON meal_plans(start_date, end_date);
```

### 5.2 Meal Entries

```sql
CREATE TABLE meal_entries (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL,
    meal_plan_id UUID REFERENCES meal_plans(id),
    
    -- Meal information
    meal_date DATE NOT NULL,
    meal_type VARCHAR(20) NOT NULL, -- 'breakfast', 'lunch', 'dinner', 'snack'
    meal_name VARCHAR(255),
    
    -- Food information
    food_id UUID REFERENCES foods(id),
    recipe_id UUID REFERENCES recipes(id),
    
    -- Quantity
    quantity DECIMAL(10,2) NOT NULL,
    unit VARCHAR(50) NOT NULL,
    quantity_in_grams DECIMAL(10,2),
    
    -- Calculated nutrition (cached)
    calories DECIMAL(8,2),
    protein_g DECIMAL(8,2),
    carbs_g DECIMAL(8,2),
    fat_g DECIMAL(8,2),
    
    -- Metadata
    notes TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    
    CONSTRAINT meal_entries_food_or_recipe CHECK (
        (food_id IS NOT NULL AND recipe_id IS NULL) OR
        (food_id IS NULL AND recipe_id IS NOT NULL)
    )
);

CREATE INDEX idx_meal_entries_user ON meal_entries(user_id);
CREATE INDEX idx_meal_entries_date ON meal_entries(meal_date);
CREATE INDEX idx_meal_entries_meal_type ON meal_entries(meal_type);
CREATE INDEX idx_meal_entries_food ON meal_entries(food_id);
CREATE INDEX idx_meal_entries_recipe ON meal_entries(recipe_id);
```

## 6. External Data Integration

### 6.1 API Data Sync

```sql
CREATE TABLE api_sync_logs (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    data_source VARCHAR(50) NOT NULL,
    sync_type VARCHAR(50) NOT NULL, -- 'full', 'incremental', 'single_item'
    external_id VARCHAR(100),
    status VARCHAR(20) NOT NULL, -- 'success', 'failed', 'partial'
    records_processed INTEGER DEFAULT 0,
    records_success INTEGER DEFAULT 0,
    records_failed INTEGER DEFAULT 0,
    error_message TEXT,
    started_at TIMESTAMP WITH TIME ZONE NOT NULL,
    completed_at TIMESTAMP WITH TIME ZONE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_api_sync_logs_source ON api_sync_logs(data_source);
CREATE INDEX idx_api_sync_logs_status ON api_sync_logs(status);
CREATE INDEX idx_api_sync_logs_date ON api_sync_logs(started_at);
```

### 6.2 Barcode Cache

```sql
CREATE TABLE barcode_cache (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    barcode VARCHAR(50) NOT NULL,
    food_id UUID REFERENCES foods(id),
    api_response JSONB,
    data_source VARCHAR(50),
    last_updated TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    is_valid BOOLEAN DEFAULT TRUE,
    
    UNIQUE(barcode)
);

CREATE INDEX idx_barcode_cache_barcode ON barcode_cache(barcode);
CREATE INDEX idx_barcode_cache_food ON barcode_cache(food_id);
CREATE INDEX idx_barcode_cache_updated ON barcode_cache(last_updated);
```

## 7. Views & Computed Data

### 7.1 Food Search View

```sql
CREATE VIEW vw_food_search AS
SELECT 
    f.id,
    f.name,
    f.name_en,
    f.name_vi,
    f.description,
    f.barcode,
    f.serving_size_g,
    f.serving_size_description,
    f.data_source,
    f.is_verified,
    f.verification_status,
    
    -- Category information
    fc.name as category_name,
    fc.name_en as category_name_en,
    fc.name_vi as category_name_vi,
    
    -- Nutrition information
    nf.calories_per_100g,
    nf.protein_g,
    nf.carbohydrates_g,
    nf.fat_g,
    nf.fiber_g,
    nf.sugar_g,
    nf.sodium_mg,
    
    -- Ratings
    COALESCE(AVG(fr.rating), 0) as average_rating,
    COUNT(fr.rating) as rating_count,
    
    f.created_at,
    f.updated_at
FROM foods f
LEFT JOIN food_categories fc ON f.category_id = fc.id
LEFT JOIN nutrition_facts nf ON f.id = nf.food_id
LEFT JOIN food_ratings fr ON f.id = fr.food_id
WHERE f.is_active = TRUE
GROUP BY f.id, fc.name, fc.name_en, fc.name_vi, nf.calories_per_100g, 
         nf.protein_g, nf.carbohydrates_g, nf.fat_g, nf.fiber_g, nf.sugar_g, nf.sodium_mg;
```

### 7.2 Daily Nutrition Summary View

```sql
CREATE VIEW vw_daily_nutrition_summary AS
SELECT 
    user_id,
    meal_date,
    SUM(calories) as total_calories,
    SUM(protein_g) as total_protein,
    SUM(carbs_g) as total_carbs,
    SUM(fat_g) as total_fat,
    COUNT(*) as meal_count,
    ARRAY_AGG(DISTINCT meal_type) as meal_types
FROM meal_entries
WHERE meal_date >= CURRENT_DATE - INTERVAL '30 days'
GROUP BY user_id, meal_date;
```

## 8. Stored Procedures & Functions

### 8.1 Calculate Recipe Nutrition

```sql
CREATE OR REPLACE FUNCTION calculate_recipe_nutrition(recipe_id_param UUID)
RETURNS TABLE (
    calories_per_serving DECIMAL(8,2),
    protein_per_serving DECIMAL(8,2),
    carbs_per_serving DECIMAL(8,2),
    fat_per_serving DECIMAL(8,2),
    fiber_per_serving DECIMAL(8,2)
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        (SUM(nf.calories_per_100g * ri.quantity_in_grams / 100) / r.servings) as calories_per_serving,
        (SUM(nf.protein_g * ri.quantity_in_grams / 100) / r.servings) as protein_per_serving,
        (SUM(nf.carbohydrates_g * ri.quantity_in_grams / 100) / r.servings) as carbs_per_serving,
        (SUM(nf.fat_g * ri.quantity_in_grams / 100) / r.servings) as fat_per_serving,
        (SUM(nf.fiber_g * ri.quantity_in_grams / 100) / r.servings) as fiber_per_serving
    FROM recipes r
    JOIN recipe_ingredients ri ON r.id = ri.recipe_id
    JOIN nutrition_facts nf ON ri.food_id = nf.food_id
    WHERE r.id = recipe_id_param
    GROUP BY r.servings;
END;
$$ LANGUAGE plpgsql;
```

### 8.2 Search Foods Function

```sql
CREATE OR REPLACE FUNCTION search_foods(
    search_term TEXT,
    category_id_param UUID DEFAULT NULL,
    limit_param INTEGER DEFAULT 20,
    offset_param INTEGER DEFAULT 0
)
RETURNS TABLE (
    id UUID,
    name VARCHAR(255),
    name_en VARCHAR(255),
    name_vi VARCHAR(255),
    category_name VARCHAR(255),
    calories_per_100g DECIMAL(8,2),
    protein_g DECIMAL(8,2),
    carbs_g DECIMAL(8,2),
    fat_g DECIMAL(8,2),
    rank REAL
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        f.id,
        f.name,
        f.name_en,
        f.name_vi,
        fc.name as category_name,
        nf.calories_per_100g,
        nf.protein_g,
        nf.carbohydrates_g,
        nf.fat_g,
        ts_rank(f.search_vector, plainto_tsquery('english', search_term)) as rank
    FROM foods f
    LEFT JOIN food_categories fc ON f.category_id = fc.id
    LEFT JOIN nutrition_facts nf ON f.id = nf.food_id
    WHERE f.is_active = TRUE
    AND f.verification_status = 'approved'
    AND (category_id_param IS NULL OR f.category_id = category_id_param)
    AND f.search_vector @@ plainto_tsquery('english', search_term)
    ORDER BY rank DESC, f.name
    LIMIT limit_param
    OFFSET offset_param;
END;
$$ LANGUAGE plpgsql;
```

## 9. Sample Data Inserts

### 9.1 Basic Categories

```sql
INSERT INTO food_categories (name, name_en, name_vi, description) VALUES
('Fruits', 'Fruits', 'Trái cây', 'Fresh and dried fruits'),
('Vegetables', 'Vegetables', 'Rau củ', 'Fresh and cooked vegetables'),
('Grains', 'Grains', 'Ngũ cốc', 'Rice, wheat, oats and other grains'),
('Proteins', 'Proteins', 'Protein', 'Meat, fish, eggs, and plant proteins'),
('Dairy', 'Dairy', 'Sữa và chế phẩm sữa', 'Milk, cheese, yogurt'),
('Vietnamese Dishes', 'Vietnamese Dishes', 'Món ăn Việt Nam', 'Traditional Vietnamese foods');
```

### 9.2 Common Allergens

```sql
INSERT INTO allergens (name, name_en, name_vi) VALUES
('Milk', 'Milk', 'Sữa'),
('Eggs', 'Eggs', 'Trứng'),
('Fish', 'Fish', 'Cá'),
('Shellfish', 'Shellfish', 'Tôm cua'),
('Tree Nuts', 'Tree Nuts', 'Hạt cây'),
('Peanuts', 'Peanuts', 'Đậu phộng'),
('Wheat', 'Wheat', 'Lúa mì'),
('Soy', 'Soy', 'Đậu nành'),
('Sesame', 'Sesame', 'Mè');
```

## 10. Performance Optimization

### 10.1 Indexes Strategy

```sql
-- Composite indexes for common queries
CREATE INDEX idx_foods_category_verified ON foods(category_id, verification_status);
CREATE INDEX idx_foods_source_active ON foods(data_source, is_active);
CREATE INDEX idx_meal_entries_user_date ON meal_entries(user_id, meal_date);
CREATE INDEX idx_meal_entries_user_type ON meal_entries(user_id, meal_type);

-- Partial indexes for specific conditions
CREATE INDEX idx_foods_verified_active ON foods(id) WHERE verification_status = 'approved' AND is_active = TRUE;
CREATE INDEX idx_foods_pending_verification ON foods(id) WHERE verification_status = 'pending';
```

### 10.2 Partitioning Strategy

```sql
-- Partition meal_entries by month for better performance
CREATE TABLE meal_entries_y2024m01 PARTITION OF meal_entries
FOR VALUES FROM ('2024-01-01') TO ('2024-02-01');

CREATE TABLE meal_entries_y2024m02 PARTITION OF meal_entries
FOR VALUES FROM ('2024-02-01') TO ('2024-03-01');

-- Continue for other months...
```

## 11. Data Validation & Constraints

### 11.1 Check Constraints

```sql
-- Add validation constraints
ALTER TABLE nutrition_facts ADD CONSTRAINT check_calories_positive 
CHECK (calories_per_100g >= 0);

ALTER TABLE nutrition_facts ADD CONSTRAINT check_protein_positive 
CHECK (protein_g >= 0);

ALTER TABLE nutrition_facts ADD CONSTRAINT check_carbs_positive 
CHECK (carbohydrates_g >= 0);

ALTER TABLE nutrition_facts ADD CONSTRAINT check_fat_positive 
CHECK (fat_g >= 0);

ALTER TABLE meal_entries ADD CONSTRAINT check_quantity_positive 
CHECK (quantity > 0);

ALTER TABLE recipe_ingredients ADD CONSTRAINT check_recipe_quantity_positive 
CHECK (quantity > 0);
```

### 11.2 Triggers for Data Integrity

```sql
-- Update timestamps trigger
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ language 'plpgsql';

CREATE TRIGGER update_foods_updated_at BEFORE UPDATE ON foods
FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_recipes_updated_at BEFORE UPDATE ON recipes
FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
```

---

**Summary**: Database design hỗ trợ full features của fitness tracking app với focus vào Vietnamese food data, user contributions, và scalability. Structure cho phép easy integration với external APIs và community-driven content. 