using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mushka.Infrastructure.DataAccess.Migrations
{
    public partial class AddExpenses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "Date", nullable: false),
                    Category = table.Column<int>(nullable: false),
                    Purpose = table.Column<string>(maxLength: 255, nullable: false),
                    Cost = table.Column<decimal>(type: "Money", nullable: false),
                    CostMethod = table.Column<int>(nullable: false),
                    SupplierName = table.Column<string>(maxLength: 255, nullable: false),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");
        }
    }
}
