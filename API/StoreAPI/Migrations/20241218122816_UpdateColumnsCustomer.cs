using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumnsCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "CustomerDetails",
                newName: "FileLocation");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "CustomerDetails",
                newName: "CustomerName");

            migrationBuilder.AddColumn<int>(
                name: "AmountPaid",
                table: "CustomerDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "CustomerDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RemainingAmount",
                table: "CustomerDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalAmount",
                table: "CustomerDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "CustomerDetails");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "CustomerDetails");

            migrationBuilder.DropColumn(
                name: "RemainingAmount",
                table: "CustomerDetails");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "CustomerDetails");

            migrationBuilder.RenameColumn(
                name: "FileLocation",
                table: "CustomerDetails",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "CustomerDetails",
                newName: "FirstName");
        }
    }
}
