using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mushka.Infrastructure.DataAccess.Migrations
{
    public partial class AddExhibition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exhibitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    FromDate = table.Column<DateTime>(type: "Date", nullable: false),
                    ToDate = table.Column<DateTime>(type: "Date", nullable: false),
                    City = table.Column<string>(nullable: false),
                    ParticipationCost = table.Column<decimal>(type: "Money", nullable: false),
                    ParticipationCostMethod = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    Profit = table.Column<decimal>(type: "Money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exhibitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExhibitionProducts",
                columns: table => new
                {
                    ExhibitionId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(type: "Money", nullable: false),
                    CostPrice = table.Column<decimal>(type: "Money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExhibitionProducts", x => new { x.ProductId, x.ExhibitionId });
                    table.ForeignKey(
                        name: "FK_ExhibitionProducts_Exhibitions_ExhibitionId",
                        column: x => x.ExhibitionId,
                        principalTable: "Exhibitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExhibitionProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExhibitionProducts_ExhibitionId",
                table: "ExhibitionProducts",
                column: "ExhibitionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExhibitionProducts");

            migrationBuilder.DropTable(
                name: "Exhibitions");
        }
    }
}
