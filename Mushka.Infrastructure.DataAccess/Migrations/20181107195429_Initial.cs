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
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    WebSite = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true)
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
                name: "ContactPersons",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 255, nullable: false),
                    LastName = table.Column<string>(maxLength: 255, nullable: false),
                    Position = table.Column<string>(maxLength: 255, nullable: true),
                    City = table.Column<string>(maxLength: 255, nullable: true),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    Phones = table.Column<string>(nullable: true),
                    SupplierId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactPersons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactPersons_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RequestDate = table.Column<DateTime>(type: "Date", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "Date", nullable: false),
                    Cost = table.Column<decimal>(type: "Money", nullable: false),
                    TransferFee = table.Column<decimal>(type: "Money", nullable: false),
                    BankFee = table.Column<decimal>(type: "Money", nullable: false),
                    SupplierId = table.Column<Guid>(nullable: true)
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
                columns: new[] { "Id", "BankFee", "Cost", "DeliveryDate", "RequestDate", "SupplierId", "TransferFee" },
                values: new object[,]
                {
                    { new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), 222.00m, 41319.00m, new DateTime(2018, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2018, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 940.00m },
                    { new Guid("32c74ef3-adfd-4723-a319-9b8984d1b7fb"), 90.00m, 16500.00m, new DateTime(2018, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2018, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 90.00m },
                    { new Guid("b2d8b13b-aa82-4820-ba85-e23501869c3a"), 110.00m, 39720.00m, new DateTime(2018, 9, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2018, 8, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 810.00m }
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
                    { new Guid("8823f027-9074-4fa9-a5ef-552a5b08ef5e"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "FLM001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Flamingo" },
                    { new Guid("76f1b29c-edac-4ca9-b529-da383c04905b"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "GRE001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deep Green" },
                    { new Guid("eabc3ce7-3c55-465e-9f27-11033bcc4f33"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "CAC001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cactus" },
                    { new Guid("d772b195-65e3-4250-8b2c-e2d59e7d24da"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "BEE001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bumble-bee" },
                    { new Guid("380e3a08-08c5-40b1-b401-ec6b57d2e549"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "SAI001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sailor" },
                    { new Guid("e536c61e-c2c5-41ec-9205-660726baa18b"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "PAS001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Royal Passion" },
                    { new Guid("5bf3988b-ba17-4802-90ad-b77abe68677a"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "LAM001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lamp" },
                    { new Guid("db645119-1b9f-4161-966d-97a7cca8d2c7"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "PEP001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pepper" },
                    { new Guid("5e838aa5-dd8c-4b6b-81ea-a0aaedf44f7d"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "BAN001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Banana" },
                    { new Guid("85ceb6f2-c29b-4809-b30a-5ccf427a0447"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "YOG001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yoga" },
                    { new Guid("09cfb881-d707-49e5-a2c1-730ce136b710"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "EIN001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Einstein" },
                    { new Guid("84c601ce-a32d-432d-99e2-c23916cf4d1f"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "STE001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jobsy" },
                    { new Guid("6bb026fc-ae0f-4a87-b0e3-845b3d55e05b"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "SBY001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Navy" },
                    { new Guid("1054708a-aa30-4ba6-84f7-321eac6aa041"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "MST001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Multi stripe" },
                    { new Guid("b62555e5-e51b-41e2-9bf8-6a750edc0d8a"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "SWR001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cherry" },
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
                    { new Guid("304af5df-1d03-40c3-af40-9c6259898f75"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "SWY001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Limono" },
                    { new Guid("297af444-055f-4b76-a3ee-fbe65b9752f6"), new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), "ORA001", new DateTime(2018, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Orange mood" }
                });

            migrationBuilder.InsertData(
                table: "DeliveryProducts",
                columns: new[] { "ProductId", "DeliveryId", "SizeId", "PriceForItem", "Quantity" },
                values: new object[,]
                {
                    { new Guid("a9ab38d1-c2b2-4c50-9ab9-80335f4561f8"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 25.0m, 39 },
                    { new Guid("5e838aa5-dd8c-4b6b-81ea-a0aaedf44f7d"), new Guid("32c74ef3-adfd-4723-a319-9b8984d1b7fb"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 27.0m, 47 },
                    { new Guid("85ceb6f2-c29b-4809-b30a-5ccf427a0447"), new Guid("32c74ef3-adfd-4723-a319-9b8984d1b7fb"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 30.0m, 53 },
                    { new Guid("85ceb6f2-c29b-4809-b30a-5ccf427a0447"), new Guid("32c74ef3-adfd-4723-a319-9b8984d1b7fb"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 30.0m, 13 },
                    { new Guid("09cfb881-d707-49e5-a2c1-730ce136b710"), new Guid("32c74ef3-adfd-4723-a319-9b8984d1b7fb"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 30.0m, 63 },
                    { new Guid("09cfb881-d707-49e5-a2c1-730ce136b710"), new Guid("32c74ef3-adfd-4723-a319-9b8984d1b7fb"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 30.0m, 53 },
                    { new Guid("84c601ce-a32d-432d-99e2-c23916cf4d1f"), new Guid("32c74ef3-adfd-4723-a319-9b8984d1b7fb"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 30.0m, 48 },
                    { new Guid("84c601ce-a32d-432d-99e2-c23916cf4d1f"), new Guid("32c74ef3-adfd-4723-a319-9b8984d1b7fb"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 30.0m, 51 },
                    { new Guid("6bb026fc-ae0f-4a87-b0e3-845b3d55e05b"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 20.0m, 62 },
                    { new Guid("6bb026fc-ae0f-4a87-b0e3-845b3d55e05b"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 20.0m, 54 },
                    { new Guid("b62555e5-e51b-41e2-9bf8-6a750edc0d8a"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 20.0m, 65 },
                    { new Guid("304af5df-1d03-40c3-af40-9c6259898f75"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 20.0m, 57 },
                    { new Guid("304af5df-1d03-40c3-af40-9c6259898f75"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 20.0m, 60 },
                    { new Guid("574f3353-0c6e-4148-a8ef-0db9559f3864"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 25.0m, 64 },
                    { new Guid("574f3353-0c6e-4148-a8ef-0db9559f3864"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 25.0m, 48 },
                    { new Guid("32ae1bae-c186-4e7c-a6af-d683e10d1480"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 25.0m, 48 },
                    { new Guid("32ae1bae-c186-4e7c-a6af-d683e10d1480"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 25.0m, 50 },
                    { new Guid("8636296d-e47c-4bb8-a6fd-e0cc01d4e27a"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 25.0m, 53 },
                    { new Guid("5e838aa5-dd8c-4b6b-81ea-a0aaedf44f7d"), new Guid("32c74ef3-adfd-4723-a319-9b8984d1b7fb"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 27.0m, 46 },
                    { new Guid("8636296d-e47c-4bb8-a6fd-e0cc01d4e27a"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 25.0m, 53 },
                    { new Guid("db645119-1b9f-4161-966d-97a7cca8d2c7"), new Guid("32c74ef3-adfd-4723-a319-9b8984d1b7fb"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 27.0m, 52 },
                    { new Guid("5bf3988b-ba17-4802-90ad-b77abe68677a"), new Guid("32c74ef3-adfd-4723-a319-9b8984d1b7fb"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 27.0m, 57 },
                    { new Guid("297af444-055f-4b76-a3ee-fbe65b9752f6"), new Guid("b2d8b13b-aa82-4820-ba85-e23501869c3a"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 25.0m, 65 },
                    { new Guid("297af444-055f-4b76-a3ee-fbe65b9752f6"), new Guid("b2d8b13b-aa82-4820-ba85-e23501869c3a"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 25.0m, 132 },
                    { new Guid("1054708a-aa30-4ba6-84f7-321eac6aa041"), new Guid("b2d8b13b-aa82-4820-ba85-e23501869c3a"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 18.7m, 160 },
                    { new Guid("1054708a-aa30-4ba6-84f7-321eac6aa041"), new Guid("b2d8b13b-aa82-4820-ba85-e23501869c3a"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 18.7m, 110 },
                    { new Guid("8823f027-9074-4fa9-a5ef-552a5b08ef5e"), new Guid("b2d8b13b-aa82-4820-ba85-e23501869c3a"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 25.5m, 114 },
                    { new Guid("8823f027-9074-4fa9-a5ef-552a5b08ef5e"), new Guid("b2d8b13b-aa82-4820-ba85-e23501869c3a"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 25.5m, 115 },
                    { new Guid("76f1b29c-edac-4ca9-b529-da383c04905b"), new Guid("b2d8b13b-aa82-4820-ba85-e23501869c3a"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 19.6m, 112 },
                    { new Guid("76f1b29c-edac-4ca9-b529-da383c04905b"), new Guid("b2d8b13b-aa82-4820-ba85-e23501869c3a"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 19.60m, 105 },
                    { new Guid("eabc3ce7-3c55-465e-9f27-11033bcc4f33"), new Guid("b2d8b13b-aa82-4820-ba85-e23501869c3a"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 25.0m, 92 },
                    { new Guid("eabc3ce7-3c55-465e-9f27-11033bcc4f33"), new Guid("b2d8b13b-aa82-4820-ba85-e23501869c3a"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 25.0m, 98 },
                    { new Guid("d772b195-65e3-4250-8b2c-e2d59e7d24da"), new Guid("b2d8b13b-aa82-4820-ba85-e23501869c3a"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 19.6m, 111 },
                    { new Guid("d772b195-65e3-4250-8b2c-e2d59e7d24da"), new Guid("b2d8b13b-aa82-4820-ba85-e23501869c3a"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 19.6m, 108 },
                    { new Guid("380e3a08-08c5-40b1-b401-ec6b57d2e549"), new Guid("b2d8b13b-aa82-4820-ba85-e23501869c3a"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 21.31m, 115 },
                    { new Guid("380e3a08-08c5-40b1-b401-ec6b57d2e549"), new Guid("b2d8b13b-aa82-4820-ba85-e23501869c3a"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 21.31m, 107 },
                    { new Guid("e536c61e-c2c5-41ec-9205-660726baa18b"), new Guid("b2d8b13b-aa82-4820-ba85-e23501869c3a"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 25.35m, 122 },
                    { new Guid("e536c61e-c2c5-41ec-9205-660726baa18b"), new Guid("b2d8b13b-aa82-4820-ba85-e23501869c3a"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 25.35m, 110 },
                    { new Guid("5bf3988b-ba17-4802-90ad-b77abe68677a"), new Guid("32c74ef3-adfd-4723-a319-9b8984d1b7fb"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 27.0m, 61 },
                    { new Guid("db645119-1b9f-4161-966d-97a7cca8d2c7"), new Guid("32c74ef3-adfd-4723-a319-9b8984d1b7fb"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 27.0m, 49 },
                    { new Guid("65801c7f-37f6-4452-a304-ceaafc940d08"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 20.0m, 51 },
                    { new Guid("b62555e5-e51b-41e2-9bf8-6a750edc0d8a"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 20.0m, 66 },
                    { new Guid("a9899cd5-9b2d-4241-8e28-0d1441933bad"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 25.0m, 44 },
                    { new Guid("f869224c-80e6-43b6-94a4-2528ecd67a75"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 25.0m, 53 },
                    { new Guid("bddc1231-0952-4c6d-9a30-9de441cfa3a0"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 25.0m, 48 },
                    { new Guid("f869224c-80e6-43b6-94a4-2528ecd67a75"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 25.0m, 53 },
                    { new Guid("ba641024-d50a-4f9c-bfd9-a330fe12071e"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 25.0m, 54 },
                    { new Guid("f9b055d3-5fd9-417f-b71d-0af81c821029"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 30.0m, 40 },
                    { new Guid("365510f0-fb1a-42cd-b249-5ad514bf2f33"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 25.0m, 46 },
                    { new Guid("f9b055d3-5fd9-417f-b71d-0af81c821029"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 30.0m, 53 },
                    { new Guid("bddc1231-0952-4c6d-9a30-9de441cfa3a0"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 25.0m, 50 },
                    { new Guid("a9899cd5-9b2d-4241-8e28-0d1441933bad"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 25.0m, 43 },
                    { new Guid("dfa69fa0-2df3-4254-95b7-f65eb4ed6c92"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 25.0m, 56 },
                    { new Guid("365510f0-fb1a-42cd-b249-5ad514bf2f33"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 25.0m, 56 },
                    { new Guid("637b0ba2-f1d9-4bf6-b1c7-1bc685033b36"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 25.0m, 58 },
                    { new Guid("a9ab38d1-c2b2-4c50-9ab9-80335f4561f8"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 25.0m, 38 },
                    { new Guid("55386f8f-3234-42c0-a82a-65ea7dd50b28"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 25.0m, 75 },
                    { new Guid("dfa69fa0-2df3-4254-95b7-f65eb4ed6c92"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 25.0m, 52 },
                    { new Guid("637b0ba2-f1d9-4bf6-b1c7-1bc685033b36"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 25.0m, 57 },
                    { new Guid("55386f8f-3234-42c0-a82a-65ea7dd50b28"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 25.0m, 57 },
                    { new Guid("ba641024-d50a-4f9c-bfd9-a330fe12071e"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 25.0m, 52 },
                    { new Guid("65801c7f-37f6-4452-a304-ceaafc940d08"), new Guid("4e50f00d-4fd9-4dfe-8d56-18a2399dd7b6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 20.0m, 52 }
                });

            migrationBuilder.InsertData(
                table: "ProductSizes",
                columns: new[] { "ProductId", "SizeId", "Quantity" },
                values: new object[,]
                {
                    { new Guid("380e3a08-08c5-40b1-b401-ec6b57d2e549"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 107 },
                    { new Guid("5bf3988b-ba17-4802-90ad-b77abe68677a"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 57 },
                    { new Guid("365510f0-fb1a-42cd-b249-5ad514bf2f33"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 46 },
                    { new Guid("365510f0-fb1a-42cd-b249-5ad514bf2f33"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 56 },
                    { new Guid("e536c61e-c2c5-41ec-9205-660726baa18b"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 122 },
                    { new Guid("380e3a08-08c5-40b1-b401-ec6b57d2e549"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 115 },
                    { new Guid("e536c61e-c2c5-41ec-9205-660726baa18b"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 110 },
                    { new Guid("5bf3988b-ba17-4802-90ad-b77abe68677a"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 61 },
                    { new Guid("65801c7f-37f6-4452-a304-ceaafc940d08"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 52 },
                    { new Guid("f9b055d3-5fd9-417f-b71d-0af81c821029"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 53 },
                    { new Guid("d772b195-65e3-4250-8b2c-e2d59e7d24da"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 108 },
                    { new Guid("d772b195-65e3-4250-8b2c-e2d59e7d24da"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 111 },
                    { new Guid("eabc3ce7-3c55-465e-9f27-11033bcc4f33"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 98 },
                    { new Guid("eabc3ce7-3c55-465e-9f27-11033bcc4f33"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 92 },
                    { new Guid("dfa69fa0-2df3-4254-95b7-f65eb4ed6c92"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 56 },
                    { new Guid("76f1b29c-edac-4ca9-b529-da383c04905b"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 105 },
                    { new Guid("76f1b29c-edac-4ca9-b529-da383c04905b"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 112 },
                    { new Guid("dfa69fa0-2df3-4254-95b7-f65eb4ed6c92"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 52 },
                    { new Guid("8823f027-9074-4fa9-a5ef-552a5b08ef5e"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 115 },
                    { new Guid("8823f027-9074-4fa9-a5ef-552a5b08ef5e"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 114 },
                    { new Guid("a9ab38d1-c2b2-4c50-9ab9-80335f4561f8"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 38 },
                    { new Guid("1054708a-aa30-4ba6-84f7-321eac6aa041"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 110 },
                    { new Guid("1054708a-aa30-4ba6-84f7-321eac6aa041"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 160 },
                    { new Guid("a9ab38d1-c2b2-4c50-9ab9-80335f4561f8"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 39 },
                    { new Guid("f9b055d3-5fd9-417f-b71d-0af81c821029"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 40 },
                    { new Guid("ba641024-d50a-4f9c-bfd9-a330fe12071e"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 52 },
                    { new Guid("5e838aa5-dd8c-4b6b-81ea-a0aaedf44f7d"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 47 },
                    { new Guid("db645119-1b9f-4161-966d-97a7cca8d2c7"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 52 },
                    { new Guid("f869224c-80e6-43b6-94a4-2528ecd67a75"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 53 },
                    { new Guid("304af5df-1d03-40c3-af40-9c6259898f75"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 57 },
                    { new Guid("304af5df-1d03-40c3-af40-9c6259898f75"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 60 },
                    { new Guid("574f3353-0c6e-4148-a8ef-0db9559f3864"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 64 },
                    { new Guid("574f3353-0c6e-4148-a8ef-0db9559f3864"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 48 },
                    { new Guid("a9899cd5-9b2d-4241-8e28-0d1441933bad"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 43 },
                    { new Guid("297af444-055f-4b76-a3ee-fbe65b9752f6"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 132 },
                    { new Guid("a9899cd5-9b2d-4241-8e28-0d1441933bad"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 44 },
                    { new Guid("32ae1bae-c186-4e7c-a6af-d683e10d1480"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 50 },
                    { new Guid("8636296d-e47c-4bb8-a6fd-e0cc01d4e27a"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 53 },
                    { new Guid("8636296d-e47c-4bb8-a6fd-e0cc01d4e27a"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 53 },
                    { new Guid("637b0ba2-f1d9-4bf6-b1c7-1bc685033b36"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 57 },
                    { new Guid("637b0ba2-f1d9-4bf6-b1c7-1bc685033b36"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 58 },
                    { new Guid("65801c7f-37f6-4452-a304-ceaafc940d08"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 51 },
                    { new Guid("32ae1bae-c186-4e7c-a6af-d683e10d1480"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 48 },
                    { new Guid("db645119-1b9f-4161-966d-97a7cca8d2c7"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 49 },
                    { new Guid("b62555e5-e51b-41e2-9bf8-6a750edc0d8a"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 65 },
                    { new Guid("f869224c-80e6-43b6-94a4-2528ecd67a75"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 53 },
                    { new Guid("ba641024-d50a-4f9c-bfd9-a330fe12071e"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 54 },
                    { new Guid("5e838aa5-dd8c-4b6b-81ea-a0aaedf44f7d"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 46 },
                    { new Guid("55386f8f-3234-42c0-a82a-65ea7dd50b28"), new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), 57 },
                    { new Guid("85ceb6f2-c29b-4809-b30a-5ccf427a0447"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 53 },
                    { new Guid("85ceb6f2-c29b-4809-b30a-5ccf427a0447"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 13 },
                    { new Guid("55386f8f-3234-42c0-a82a-65ea7dd50b28"), new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), 75 },
                    { new Guid("b62555e5-e51b-41e2-9bf8-6a750edc0d8a"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 66 },
                    { new Guid("09cfb881-d707-49e5-a2c1-730ce136b710"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 63 },
                    { new Guid("bddc1231-0952-4c6d-9a30-9de441cfa3a0"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 50 },
                    { new Guid("84c601ce-a32d-432d-99e2-c23916cf4d1f"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 48 },
                    { new Guid("84c601ce-a32d-432d-99e2-c23916cf4d1f"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 51 },
                    { new Guid("bddc1231-0952-4c6d-9a30-9de441cfa3a0"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 48 },
                    { new Guid("6bb026fc-ae0f-4a87-b0e3-845b3d55e05b"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 62 },
                    { new Guid("6bb026fc-ae0f-4a87-b0e3-845b3d55e05b"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 54 },
                    { new Guid("09cfb881-d707-49e5-a2c1-730ce136b710"), new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), 53 },
                    { new Guid("297af444-055f-4b76-a3ee-fbe65b9752f6"), new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), 65 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactPersons_SupplierId",
                table: "ContactPersons",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_SupplierId",
                table: "Deliveries",
                column: "SupplierId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Name",
                table: "Suppliers",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactPersons");

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
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
