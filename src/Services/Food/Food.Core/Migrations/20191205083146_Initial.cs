using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Food.Core.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Foods",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Facts_Protein = table.Column<decimal>(nullable: true),
                    Facts_Carbohydrates = table.Column<decimal>(nullable: true),
                    Facts_Fat = table.Column<decimal>(nullable: true),
                    Facts_Calories = table.Column<decimal>(nullable: true),
                    FoodType = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meals",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IntakeTime = table.Column<DateTime>(nullable: false),
                    Facts_Protein = table.Column<decimal>(nullable: true),
                    Facts_Carbohydrates = table.Column<decimal>(nullable: true),
                    Facts_Fat = table.Column<decimal>(nullable: true),
                    Facts_Calories = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ingridients",
                schema: "public",
                columns: table => new
                {
                    RecipeId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingridients", x => new { x.ProductId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_Ingridients_Foods_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "public",
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ingridients_Foods_RecipeId",
                        column: x => x.RecipeId,
                        principalSchema: "public",
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealItems",
                schema: "public",
                columns: table => new
                {
                    MealId = table.Column<int>(nullable: false),
                    FoodId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Facts_Protein = table.Column<decimal>(nullable: true),
                    Facts_Carbohydrates = table.Column<decimal>(nullable: true),
                    Facts_Fat = table.Column<decimal>(nullable: true),
                    Facts_Calories = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealItems", x => new { x.MealId, x.FoodId });
                    table.ForeignKey(
                        name: "FK_MealItems_Foods_FoodId",
                        column: x => x.FoodId,
                        principalSchema: "public",
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealItems_Meals_MealId",
                        column: x => x.MealId,
                        principalSchema: "public",
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingridients_RecipeId",
                schema: "public",
                table: "Ingridients",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_MealItems_FoodId",
                schema: "public",
                table: "MealItems",
                column: "FoodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ingridients",
                schema: "public");

            migrationBuilder.DropTable(
                name: "MealItems",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Foods",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Meals",
                schema: "public");
        }
    }
}
