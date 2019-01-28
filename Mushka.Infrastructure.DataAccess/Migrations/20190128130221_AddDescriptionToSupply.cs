using Microsoft.EntityFrameworkCore.Migrations;

namespace Mushka.Infrastructure.DataAccess.Migrations
{
    public partial class AddDescriptionToSupply : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Supplies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Supplies");
        }
    }
}
