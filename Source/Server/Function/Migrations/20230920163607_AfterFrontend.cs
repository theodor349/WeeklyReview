using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Functions.Migrations;

/// <inheritdoc />
public partial class AfterFrontend : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Activity_Category_CategoryId",
            table: "Activity");

        migrationBuilder.AlterColumn<int>(
            name: "CategoryId",
            table: "Activity",
            type: "int",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AddForeignKey(
            name: "FK_Activity_Category_CategoryId",
            table: "Activity",
            column: "CategoryId",
            principalTable: "Category",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Activity_Category_CategoryId",
            table: "Activity");

        migrationBuilder.AlterColumn<int>(
            name: "CategoryId",
            table: "Activity",
            type: "int",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AddForeignKey(
            name: "FK_Activity_Category_CategoryId",
            table: "Activity",
            column: "CategoryId",
            principalTable: "Category",
            principalColumn: "Id");
    }
}
