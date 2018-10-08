using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Mushka.Accounting.Infrastructure.DataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsSizesRequired = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Sizes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    ContactPerson = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    PaymentConditions = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: false),
                    Services = table.Column<string>(nullable: true),
                    WebSite = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    DeliveriesNumber = table.Column<int>(nullable: false),
                    LastDeliveryCount = table.Column<int>(nullable: false),
                    LastDeliveryDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    TotalCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BatchNumber = table.Column<string>(nullable: false),
                    DeliveryCost = table.Column<decimal>(nullable: false),
                    DeliveryDate = table.Column<DateTime>(nullable: false),
                    IsDraft = table.Column<bool>(nullable: false),
                    PaymentMethod = table.Column<int>(nullable: false),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    SupplierId = table.Column<Guid>(nullable: true),
                    TotalCost = table.Column<decimal>(nullable: false),
                    TransferFee = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deliveries_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryProducts",
                columns: table => new
                {
                    DeliveryId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    CostPerItem = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryProducts", x => new { x.DeliveryId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_DeliveryProducts_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "Deliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryService",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Cost = table.Column<int>(nullable: false),
                    DeliveryId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryService_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "Deliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SizeItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    DeliveryProductDeliveryId = table.Column<Guid>(nullable: true),
                    DeliveryProductProductId = table.Column<Guid>(nullable: true),
                    ProductId = table.Column<Guid>(nullable: true),
                    Size = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SizeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SizeItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SizeItems_DeliveryProducts_DeliveryProductDeliveryId_DeliveryProductProductId",
                        columns: x => new { x.DeliveryProductDeliveryId, x.DeliveryProductProductId },
                        principalTable: "DeliveryProducts",
                        principalColumns: new[] { "DeliveryId", "ProductId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_SupplierId",
                table: "Deliveries",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryProducts_ProductId",
                table: "DeliveryProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryService_DeliveryId",
                table: "DeliveryService",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SizeItems_ProductId",
                table: "SizeItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SizeItems_DeliveryProductDeliveryId_DeliveryProductProductId",
                table: "SizeItems",
                columns: new[] { "DeliveryProductDeliveryId", "DeliveryProductProductId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryService");

            migrationBuilder.DropTable(
                name: "SizeItems");

            migrationBuilder.DropTable(
                name: "DeliveryProducts");

            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
