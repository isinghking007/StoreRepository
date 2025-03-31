using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddBahiKhataTable : Migration
    {
        /// <inheritdoc />
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "BahiKhata",
               columns: table => new
               {
                   DueId = table.Column<int>(type: "int", nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   CustomerId = table.Column<int>(type: "int", nullable: false),
                   TotalBillAmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   NewAmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   PaidAmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   FileLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_BahiKhata", x => x.DueId);
               });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BahiKhata");
        }
    }
}
