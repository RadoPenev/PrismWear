using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrismWear.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedByUserInProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddedByUser",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_AddedByUser",
                table: "Products",
                column: "AddedByUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_AddedByUser",
                table: "Products",
                column: "AddedByUser",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_AddedByUser",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_AddedByUser",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AddedByUser",
                table: "Products");
        }
    }
}
