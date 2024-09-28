using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Functions.Migrations;

/// <inheritdoc />
public partial class UniqueColumnIndex : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_Category_UserGuid_NormalizedName",
            table: "Category",
            columns: new[] { "UserGuid", "NormalizedName" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Activity_UserGuid_NormalizedName",
            table: "Activity",
            columns: new[] { "UserGuid", "NormalizedName" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Category_UserGuid_NormalizedName",
            table: "Category");

        migrationBuilder.DropIndex(
            name: "IX_Activity_UserGuid_NormalizedName",
            table: "Activity");
    }
}
