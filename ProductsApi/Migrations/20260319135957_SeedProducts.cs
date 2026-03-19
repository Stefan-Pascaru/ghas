using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductsApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Name", "Description", "Price", "Inventory" },
                values: new object[,]
                {
                    { "Laptop Pro 15", "High-performance laptop with 15-inch display", 1299.99m, 50 },
                    { "Wireless Mouse", "Ergonomic wireless mouse with long battery life", 29.99m, 200 },
                    { "USB-C Hub", "7-in-1 USB-C hub with HDMI and card reader", 49.99m, 150 },
                    { "Mechanical Keyboard", "TKL mechanical keyboard with RGB backlighting", 89.99m, 75 },
                    { "27\" Monitor", "4K UHD monitor with HDR support", 399.99m, 30 },
                    { "Webcam HD", "1080p webcam with built-in microphone", 59.99m, 120 },
                    { "Headphones Pro", "Noise-cancelling over-ear headphones", 149.99m, 60 },
                    { "External SSD 1TB", "Portable SSD with 1TB storage and USB 3.2", 109.99m, 80 },
                    { "Laptop Stand", "Adjustable aluminum laptop stand", 39.99m, 180 },
                    { "Desk Lamp LED", "Dimmable LED desk lamp with USB charging port", 34.99m, 100 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Products WHERE Name IN ('Laptop Pro 15', 'Wireless Mouse', 'USB-C Hub', 'Mechanical Keyboard', '27\" Monitor', 'Webcam HD', 'Headphones Pro', 'External SSD 1TB', 'Laptop Stand', 'Desk Lamp LED')");
        }
    }
}
