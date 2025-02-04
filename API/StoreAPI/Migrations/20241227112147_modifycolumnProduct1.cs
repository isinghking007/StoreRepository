using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class modifycolumnProduct1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductDescription",
                table: "ProductDetails");

            migrationBuilder.RenameColumn(
                name: "TotalVolume",
                table: "ProductDetails",
                newName: "TotalQuantity");

            migrationBuilder.RenameColumn(
                name: "SalePrice",
                table: "ProductDetails",
                newName: "SellingPrice");

            migrationBuilder.RenameColumn(
                name: "AvailableVolume",
                table: "ProductDetails",
                newName: "ProductMRP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalQuantity",
                table: "ProductDetails",
                newName: "TotalVolume");

            migrationBuilder.RenameColumn(
                name: "SellingPrice",
                table: "ProductDetails",
                newName: "SalePrice");

            migrationBuilder.RenameColumn(
                name: "ProductMRP",
                table: "ProductDetails",
                newName: "AvailableVolume");

            migrationBuilder.AddColumn<string>(
                name: "ProductDescription",
                table: "ProductDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
