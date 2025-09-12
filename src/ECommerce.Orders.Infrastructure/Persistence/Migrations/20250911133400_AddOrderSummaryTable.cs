using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Orders.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderSummaryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderSummary",
                table: "OrderSummary");

            migrationBuilder.RenameTable(
                name: "OrderSummary",
                newName: "OrderSummaries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderSummaries",
                table: "OrderSummaries",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderSummaries",
                table: "OrderSummaries");

            migrationBuilder.RenameTable(
                name: "OrderSummaries",
                newName: "OrderSummary");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderSummary",
                table: "OrderSummary",
                column: "Id");
        }
    }
}
