using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseSystem.Migrations
{
    /// <inheritdoc />
    public partial class AlterExpenseDetailFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "ExpenseDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                table: "ExpenseDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StoreName",
                table: "ExpenseDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "ExpenseDetails");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "ExpenseDetails");

            migrationBuilder.DropColumn(
                name: "StoreName",
                table: "ExpenseDetails");
        }
    }
}
