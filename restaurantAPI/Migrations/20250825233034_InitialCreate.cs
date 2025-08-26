using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace restaurantAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__19093A2BFB989CE2", x => x.CategoryID);
                });

            // Insert initial categories
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryID", "CategoryName", "Description" },
                values: new object[,]
                {
                    { 1, "Electronics", "Devices and gadgets" },
                    { 2, "Furniture", "Home & Office items" },
                    { 3, "Kitchen", "Cooking accessories" }
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    OrderDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Orders__C3905BAFD6E8DEB1", x => x.OrderID);
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderID", "CustomerName", "OrderDate", "TotalAmount" },
                values: new object[,]
                {
                    { 1, "Alice Brown", new DateOnly(2025, 8, 18), 960.00m },
                    { 2, "John Smith", new DateOnly(2025, 8, 19), 720.00m }
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Products__B40CC6EDF4042A6C", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK__Products__Catego__3B75D760",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID");
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductID", "ProductName", "CategoryID", "Price", "StockQuantity" },
                values: new object[,]
                {
                    { 1, "Laptop", 2, 900.00m, 25 },
                    { 2, "Phone", 3, 600.00m, 50 },
                    { 3, "Desk Chair", 2, 120.00m, 40 },
                    { 4, "Coffee Mug", 1, 10.00m, 200 }
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderDet__D3B9D30C5747DC46", x => x.OrderDetailID);
                    table.ForeignKey(
                        name: "FK__OrderDeta__Order__403A8C7D",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__OrderDeta__Produ__412EB0B6",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "OrderDetailID", "OrderID", "ProductID", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, 900.00m },
                    { 2, 1, 4, 6, 10.00m },
                    { 3, 2, 2, 1, 600.00m },
                    { 4, 2, 3, 1, 120.00m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderID",
                table: "OrderDetails",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductID",
                table: "OrderDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
