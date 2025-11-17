using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.WPF.Migrations
{
    /// <inheritdoc />
    public partial class AllowNullAIPrompt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AIPrompt",
                table: "WorkoutPlans",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "AIResponse",
                table: "NutritionSuggestions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "AIPrompt",
                table: "NutritionSuggestions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WorkoutPlans",
                keyColumn: "AIPrompt",
                keyValue: null,
                column: "AIPrompt",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "AIPrompt",
                table: "WorkoutPlans",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "NutritionSuggestions",
                keyColumn: "AIResponse",
                keyValue: null,
                column: "AIResponse",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "AIResponse",
                table: "NutritionSuggestions",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "NutritionSuggestions",
                keyColumn: "AIPrompt",
                keyValue: null,
                column: "AIPrompt",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "AIPrompt",
                table: "NutritionSuggestions",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
