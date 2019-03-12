using Microsoft.EntityFrameworkCore.Migrations;

namespace Mushka.Infrastructure.DataAccess.Migrations
{
    public partial class AddIsArchivalAndIsAdditional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchival",
                table: "Products",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdditional",
                table: "Categories",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchival",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsAdditional",
                table: "Categories");
        }
    }
}
