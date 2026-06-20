using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseSystem.Migrations
{
    /// <inheritdoc />
    public partial class AlterExpenseDetailReceiptType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReceiptType",
                table: "ExpenseDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiptType",
                table: "ExpenseDetails");
        }
    }
}
