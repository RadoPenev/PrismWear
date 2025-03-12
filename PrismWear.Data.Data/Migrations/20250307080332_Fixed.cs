using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrismWear.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingAddressLine2",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "OrderItems");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddressLine2",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
