using Microsoft.EntityFrameworkCore.Migrations;

namespace Mushka.Infrastructure.DataAccess.Migrations
{
    public partial class MoveCityAndRegionFromCustomerToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Orders",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Orders",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                UPDATE [Orders]
                SET
                    Region = cust.Region,
                    City = cust.City
                FROM [Orders] ord
                    INNER JOIN [Customers] cust ON cust.Id = ord.CustomerId");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Customers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Customers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Customers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                UPDATE [Customers]
                SET
                    Region = ord.Region,
                    City = ord.City
                FROM [Orders] ord
                    INNER JOIN [Customers] cust ON cust.Id = ord.CustomerId");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Orders");
        }
    }
}
