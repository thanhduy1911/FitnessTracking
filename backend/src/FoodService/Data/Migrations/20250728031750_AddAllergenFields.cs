using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAllergenFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "category",
                table: "allergens",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "color_hex",
                table: "allergens",
                type: "character varying(7)",
                maxLength: 7,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "icon_url",
                table: "allergens",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_common_in_vietnamese_food",
                table: "allergens",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "severity_level",
                table: "allergens",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "allergens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "category",
                table: "allergens");

            migrationBuilder.DropColumn(
                name: "color_hex",
                table: "allergens");

            migrationBuilder.DropColumn(
                name: "icon_url",
                table: "allergens");

            migrationBuilder.DropColumn(
                name: "is_common_in_vietnamese_food",
                table: "allergens");

            migrationBuilder.DropColumn(
                name: "severity_level",
                table: "allergens");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "allergens");
        }
    }
}
