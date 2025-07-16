using Microsoft.EntityFrameworkCore;
using FoodService.Data;
using FoodService.Services;
using FoodService.Extensions;

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
await DbInitializer.InitDbAsync(app);

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.Run();
