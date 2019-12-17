using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

namespace Food.Core.Migrations
{
    public partial class FoodSearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                schema: "public",
                table: "Foods",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Foods_SearchVector",
                schema: "public",
                table: "Foods",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.Sql(
                @"CREATE TRIGGER food_search_vector_update BEFORE INSERT OR UPDATE
              ON ""Foods"" FOR EACH ROW EXECUTE PROCEDURE
              tsvector_update_trigger(""SearchVector"", 'pg_catalog.russian', ""Name"");");

            migrationBuilder.Sql("UPDATE \"Foods\" SET \"Name\" = \"Name\";");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER product_search_vector_update");

            migrationBuilder.DropIndex(
                name: "IX_Foods_SearchVector",
                schema: "public",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "SearchVector",
                schema: "public",
                table: "Foods");
        }
    }
}
