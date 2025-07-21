using FoodService.Data;
using FoodService.Extensions;
using FoodService.Services;
using FoodService.Services.Implementations;
using FoodService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Entity Framework
builder.Services.AddDbContext<FoodDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add AutoMapper with all mapping profiles
builder.Services.AddAutoMapperWithConfiguration();

// Add application services
builder.Services.AddScoped<FoodSeedService>();
builder.Services.AddScoped<ServingSizeService>();

// Add business services
builder.Services.AddScoped<IFoodService, FoodService.Services.Implementations.FoodService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<INutritionCalculationService, NutritionCalculationService>();

// Add API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Initialize database
try
{
    await DbInitializer.InitDbAsync(app);
}
catch (Exception e)
{
    Console.WriteLine(e);
}

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.Run();
