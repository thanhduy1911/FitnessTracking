using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColorHexToFoodCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "color_hex",
                table: "food_categories",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "color_hex",
                table: "food_categories");
        }
    }
}
