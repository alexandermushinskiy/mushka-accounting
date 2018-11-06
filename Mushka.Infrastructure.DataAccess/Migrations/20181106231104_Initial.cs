using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mushka.Infrastructure.DataAccess.Migrations
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
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RequestDate = table.Column<DateTime>(type: "Date", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "Date", nullable: false),
                    PaymentMethod = table.Column<int>(nullable: false),
                    Cost = table.Column<decimal>(type: "Money", nullable: false),
                    TransferFee = table.Column<decimal>(type: "Money", nullable: false),
                    BankFee = table.Column<decimal>(type: "Money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrderDate = table.Column<DateTime>(type: "Date", nullable: false),
                    PaymentMethod = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sizes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryProducts",
                columns: table => new
                {
                    DeliveryId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    SizeId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    PriceForItem = table.Column<decimal>(type: "Money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryProducts", x => new { x.ProductId, x.DeliveryId, x.SizeId });
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
                    table.ForeignKey(
                        name: "FK_DeliveryProducts_Sizes_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Sizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderProducts",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    SizeId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    PriceForItem = table.Column<decimal>(type: "Money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProducts", x => new { x.ProductId, x.OrderId, x.SizeId });
                    table.ForeignKey(
                        name: "FK_OrderProducts_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProducts_Sizes_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Sizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSizes",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(nullable: false),
                    SizeId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSizes", x => new { x.ProductId, x.SizeId });
                    table.ForeignKey(
                        name: "FK_ProductSizes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSizes_Sizes_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Sizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "Socks" },
                    { new Guid("0e7be1de-267c-4c0a-8ee9-aba0a267f27a"), "Pack" },
                    { new Guid("b425d75b-2e72-45f0-a55d-3ba400051e5f"), "Other" }
                });

            migrationBuilder.InsertData(
                table: "Deliveries",
                columns: new[] { "Id", "BankFee", "Cost", "DeliveryDate", "PaymentMethod", "RequestDate", "TransferFee" },
                values: new object[,]
                {
                    { new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), 222.00m, 41319.00m, new DateTime(2018, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2018, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 940.00m },
                    { new Guid("32c74ef3-adfd-4723-a319-9b8984d1b7fb"), 90.00m, 16500.00m, new DateTime(2018, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2018, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 90.00m },
                    { new Guid("b2d8b13b-aa82-4820-ba85-e23501869c3a"), 110.00m, 39720.00m, new DateTime(2018, 9, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2018, 8, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 810.00m }
                });

            migrationBuilder.InsertData(
                table: "Sizes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), "36-39" },
                    { new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), "41-45" },
                    { new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), "39-42" },
                    { new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), "43-46" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Code", "CreatedOn", "Name" },
                values: new object[,]
                {
                    { new Guid("a9ab38d1-c2b2-4c50-9ab9-80335f4561f8"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "AIR001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Airplane" },
                    { new Guid("b62555e5-e51b-41e2-9bf8-6a750edc0d8a"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "SWR001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cherry" },
                    { new Guid("304af5df-1d03-40c3-af40-9c6259898f75"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "SWY001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Limono" },
                    { new Guid("574f3353-0c6e-4148-a8ef-0db9559f3864"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "GAL001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spaceman" },
                    { new Guid("32ae1bae-c186-4e7c-a6af-d683e10d1480"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "DOW001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Apelsinka" },
                    { new Guid("8636296d-e47c-4bb8-a6fd-e0cc01d4e27a"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "DRW001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Beboss dot" },
                    { new Guid("65801c7f-37f6-4452-a304-ceaafc940d08"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "DGY001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Avo-avocado" },
                    { new Guid("637b0ba2-f1d9-4bf6-b1c7-1bc685033b36"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "CFF001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "CoffeeOk" },
                    { new Guid("a9899cd5-9b2d-4241-8e28-0d1441933bad"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "CAM001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Smile" },
                    { new Guid("f869224c-80e6-43b6-94a4-2528ecd67a75"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "BER001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Beer" },
                    { new Guid("bddc1231-0952-4c6d-9a30-9de441cfa3a0"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "BDM001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Badminton" },
                    { new Guid("55386f8f-3234-42c0-a82a-65ea7dd50b28"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "DRJ001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yoda" },
                    { new Guid("ba641024-d50a-4f9c-bfd9-a330fe12071e"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "DRV001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shturmovik" },
                    { new Guid("365510f0-fb1a-42cd-b249-5ad514bf2f33"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "DRT001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dartik" },
                    { new Guid("f9b055d3-5fd9-417f-b71d-0af81c821029"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "MOT001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Motobike" },
                    { new Guid("dfa69fa0-2df3-4254-95b7-f65eb4ed6c92"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "BIK001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bike" },
                    { new Guid("6bb026fc-ae0f-4a87-b0e3-845b3d55e05b"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "SBY001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Navy" },
                    { new Guid("a6f3cc9a-bd32-49f5-8a5e-cad1262298f8"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "" }
                });

            migrationBuilder.InsertData(
                table: "DeliveryProducts",
                columns: new[] { "ProductId", "DeliveryId", "SizeId", "PriceForItem", "Quantity" },
                values: new object[,]
                {
                    { new Guid("a9ab38d1-c2b2-4c50-9ab9-80335f4561f8"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 25.0m, 39 },
                    { new Guid("f869224c-80e6-43b6-94a4-2528ecd67a75"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 25.0m, 53 },
                    { new Guid("f869224c-80e6-43b6-94a4-2528ecd67a75"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 25.0m, 53 },
                    { new Guid("a9899cd5-9b2d-4241-8e28-0d1441933bad"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 25.0m, 43 },
                    { new Guid("637b0ba2-f1d9-4bf6-b1c7-1bc685033b36"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 25.0m, 57 },
                    { new Guid("637b0ba2-f1d9-4bf6-b1c7-1bc685033b36"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 25.0m, 58 },
                    { new Guid("65801c7f-37f6-4452-a304-ceaafc940d08"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 20.0m, 52 },
                    { new Guid("65801c7f-37f6-4452-a304-ceaafc940d08"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 20.0m, 51 },
                    { new Guid("8636296d-e47c-4bb8-a6fd-e0cc01d4e27a"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 25.0m, 53 },
                    { new Guid("8636296d-e47c-4bb8-a6fd-e0cc01d4e27a"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 25.0m, 53 },
                    { new Guid("32ae1bae-c186-4e7c-a6af-d683e10d1480"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 25.0m, 50 },
                    { new Guid("32ae1bae-c186-4e7c-a6af-d683e10d1480"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 25.0m, 48 },
                    { new Guid("574f3353-0c6e-4148-a8ef-0db9559f3864"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 25.0m, 48 },
                    { new Guid("574f3353-0c6e-4148-a8ef-0db9559f3864"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 25.0m, 64 },
                    { new Guid("304af5df-1d03-40c3-af40-9c6259898f75"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 20.0m, 60 },
                    { new Guid("304af5df-1d03-40c3-af40-9c6259898f75"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 20.0m, 57 },
                    { new Guid("b62555e5-e51b-41e2-9bf8-6a750edc0d8a"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 20.0m, 65 },
                    { new Guid("b62555e5-e51b-41e2-9bf8-6a750edc0d8a"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 20.0m, 66 },
                    { new Guid("6bb026fc-ae0f-4a87-b0e3-845b3d55e05b"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 20.0m, 54 },
                    { new Guid("6bb026fc-ae0f-4a87-b0e3-845b3d55e05b"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 20.0m, 62 },
                    { new Guid("bddc1231-0952-4c6d-9a30-9de441cfa3a0"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 25.0m, 48 },
                    { new Guid("bddc1231-0952-4c6d-9a30-9de441cfa3a0"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 25.0m, 50 },
                    { new Guid("a9899cd5-9b2d-4241-8e28-0d1441933bad"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 25.0m, 44 },
                    { new Guid("dfa69fa0-2df3-4254-95b7-f65eb4ed6c92"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 25.0m, 56 },
                    { new Guid("55386f8f-3234-42c0-a82a-65ea7dd50b28"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 25.0m, 75 },
                    { new Guid("55386f8f-3234-42c0-a82a-65ea7dd50b28"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 25.0m, 57 },
                    { new Guid("a9ab38d1-c2b2-4c50-9ab9-80335f4561f8"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 25.0m, 38 },
                    { new Guid("365510f0-fb1a-42cd-b249-5ad514bf2f33"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 25.0m, 56 },
                    { new Guid("f9b055d3-5fd9-417f-b71d-0af81c821029"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 30.0m, 53 },
                    { new Guid("dfa69fa0-2df3-4254-95b7-f65eb4ed6c92"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 25.0m, 52 },
                    { new Guid("ba641024-d50a-4f9c-bfd9-a330fe12071e"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 25.0m, 54 },
                    { new Guid("365510f0-fb1a-42cd-b249-5ad514bf2f33"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 25.0m, 46 },
                    { new Guid("f9b055d3-5fd9-417f-b71d-0af81c821029"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 30.0m, 40 },
                    { new Guid("ba641024-d50a-4f9c-bfd9-a330fe12071e"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 25.0m, 52 }
                });

            migrationBuilder.InsertData(
                table: "ProductSizes",
                columns: new[] { "ProductId", "SizeId", "Quantity" },
                values: new object[,]
                {
                    { new Guid("32ae1bae-c186-4e7c-a6af-d683e10d1480"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 50 },
                    { new Guid("32ae1bae-c186-4e7c-a6af-d683e10d1480"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 48 },
                    { new Guid("dfa69fa0-2df3-4254-95b7-f65eb4ed6c92"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 56 },
                    { new Guid("574f3353-0c6e-4148-a8ef-0db9559f3864"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 48 },
                    { new Guid("55386f8f-3234-42c0-a82a-65ea7dd50b28"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 75 },
                    { new Guid("dfa69fa0-2df3-4254-95b7-f65eb4ed6c92"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 52 },
                    { new Guid("304af5df-1d03-40c3-af40-9c6259898f75"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 60 },
                    { new Guid("304af5df-1d03-40c3-af40-9c6259898f75"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 57 },
                    { new Guid("a9ab38d1-c2b2-4c50-9ab9-80335f4561f8"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 38 },
                    { new Guid("b62555e5-e51b-41e2-9bf8-6a750edc0d8a"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 65 },
                    { new Guid("b62555e5-e51b-41e2-9bf8-6a750edc0d8a"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 66 },
                    { new Guid("a9ab38d1-c2b2-4c50-9ab9-80335f4561f8"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 39 },
                    { new Guid("574f3353-0c6e-4148-a8ef-0db9559f3864"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 64 },
                    { new Guid("f9b055d3-5fd9-417f-b71d-0af81c821029"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 53 },
                    { new Guid("f9b055d3-5fd9-417f-b71d-0af81c821029"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 40 },
                    { new Guid("8636296d-e47c-4bb8-a6fd-e0cc01d4e27a"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 53 },
                    { new Guid("bddc1231-0952-4c6d-9a30-9de441cfa3a0"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 50 },
                    { new Guid("bddc1231-0952-4c6d-9a30-9de441cfa3a0"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 48 },
                    { new Guid("ba641024-d50a-4f9c-bfd9-a330fe12071e"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 54 },
                    { new Guid("ba641024-d50a-4f9c-bfd9-a330fe12071e"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 52 },
                    { new Guid("f869224c-80e6-43b6-94a4-2528ecd67a75"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 53 },
                    { new Guid("f869224c-80e6-43b6-94a4-2528ecd67a75"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 53 },
                    { new Guid("6bb026fc-ae0f-4a87-b0e3-845b3d55e05b"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 54 },
                    { new Guid("8636296d-e47c-4bb8-a6fd-e0cc01d4e27a"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 53 },
                    { new Guid("a9899cd5-9b2d-4241-8e28-0d1441933bad"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 43 },
                    { new Guid("365510f0-fb1a-42cd-b249-5ad514bf2f33"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 46 },
                    { new Guid("637b0ba2-f1d9-4bf6-b1c7-1bc685033b36"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 57 },
                    { new Guid("637b0ba2-f1d9-4bf6-b1c7-1bc685033b36"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 58 },
                    { new Guid("365510f0-fb1a-42cd-b249-5ad514bf2f33"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 56 },
                    { new Guid("65801c7f-37f6-4452-a304-ceaafc940d08"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 52 },
                    { new Guid("65801c7f-37f6-4452-a304-ceaafc940d08"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 51 },
                    { new Guid("55386f8f-3234-42c0-a82a-65ea7dd50b28"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 57 },
                    { new Guid("a9899cd5-9b2d-4241-8e28-0d1441933bad"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 44 },
                    { new Guid("6bb026fc-ae0f-4a87-b0e3-845b3d55e05b"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 62 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryProducts_DeliveryId",
                table: "DeliveryProducts",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryProducts_SizeId",
                table: "DeliveryProducts",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_OrderId",
                table: "OrderProducts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_SizeId",
                table: "OrderProducts",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSizes_SizeId",
                table: "ProductSizes",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_Name",
                table: "Sizes",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryProducts");

            migrationBuilder.DropTable(
                name: "OrderProducts");

            migrationBuilder.DropTable(
                name: "ProductSizes");

            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Sizes");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
