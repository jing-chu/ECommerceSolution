using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Orders.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SoftDeleteToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Products");
        }
    }
}
