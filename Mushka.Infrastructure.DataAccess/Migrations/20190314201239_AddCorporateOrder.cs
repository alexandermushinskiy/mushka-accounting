using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mushka.Infrastructure.DataAccess.Migrations
{
    public partial class AddCorporateOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CorporateOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "Date", nullable: false),
                    Cost = table.Column<decimal>(type: "Money", nullable: false),
                    CostMethod = table.Column<int>(nullable: false),
                    Prepayment = table.Column<decimal>(type: "Money", nullable: true),
                    PrepaymentMethod = table.Column<int>(nullable: true),
                    DeliveryCost = table.Column<decimal>(type: "Money", nullable: true),
                    DeliveryCostMethod = table.Column<int>(nullable: true),
                    Tax = table.Column<int>(nullable: true),
                    Profit = table.Column<decimal>(type: "Money", nullable: false),
                    CompanyName = table.Column<string>(maxLength: 255, nullable: false),
                    ContactPerson = table.Column<string>(maxLength: 255, nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: true),
                    Phone = table.Column<string>(maxLength: 255, nullable: false),
                    City = table.Column<string>(maxLength: 255, nullable: false),
                    Region = table.Column<string>(maxLength: 255, nullable: false),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorporateOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CorporateOrderProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(type: "Money", nullable: false),
                    CostPrice = table.Column<decimal>(type: "Money", nullable: false),
                    CorporateOrderId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorporateOrderProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorporateOrderProducts_CorporateOrders_CorporateOrderId",
                        column: x => x.CorporateOrderId,
                        principalTable: "CorporateOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorporateOrderProducts_CorporateOrderId",
                table: "CorporateOrderProducts",
                column: "CorporateOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CorporateOrders_Number",
                table: "CorporateOrders",
                column: "Number",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CorporateOrderProducts");

            migrationBuilder.DropTable(
                name: "CorporateOrders");
        }
    }
}
