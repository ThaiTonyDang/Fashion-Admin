using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FashionWeb.Infrastructure.Migrations
{
    public partial class RenameProductEnableColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Discontinued",
                table: "Products",
                newName: "Enable");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Enable",
                table: "Products",
                newName: "Discontinued");
        }
    }
}
