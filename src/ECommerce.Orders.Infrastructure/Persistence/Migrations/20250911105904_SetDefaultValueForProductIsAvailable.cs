using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Orders.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SetDefaultValueForProductIsAvailable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsAvailable",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.Sql("UPDATE Products SET IsAvailable = 1 WHERE IsAvailable = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsAvailable",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: true);
        }
    }
}
