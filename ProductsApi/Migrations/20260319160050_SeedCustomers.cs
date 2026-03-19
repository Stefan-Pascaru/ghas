using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductsApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedCustomers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert 5 customers
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Name" },
                values: new object[,]
                {
                    { "Alice Johnson" },
                    { "Bob Smith" },
                    { "Carol White" },
                    { "David Brown" },
                    { "Eva Martinez" }
                });

            // Insert 1 agreed product price per customer using product name lookups
            migrationBuilder.Sql(@"
                INSERT INTO CustomerProductPrices (CustomerId, ProductId, AgreedPrice)
                SELECT c.Id, p.Id, cpp.AgreedPrice
                FROM (VALUES
                    ('Alice Johnson',  'Laptop Pro 15',      1199.99),
                    ('Bob Smith',      'Wireless Mouse',       24.99),
                    ('Carol White',    'Mechanical Keyboard',  79.99),
                    ('David Brown',    '27"" Monitor',        369.99),
                    ('Eva Martinez',   'Headphones Pro',      129.99)
                ) AS cpp(CustomerName, ProductName, AgreedPrice)
                JOIN Customers c ON c.Name = cpp.CustomerName
                JOIN Products  p ON p.Name = cpp.ProductName
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE cpp
                FROM CustomerProductPrices cpp
                JOIN Customers c ON c.Id = cpp.CustomerId
                WHERE c.Name IN ('Alice Johnson', 'Bob Smith', 'Carol White', 'David Brown', 'Eva Martinez')
            ");

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Name",
                keyColumnType: "nvarchar(max)",
                keyValue: "Alice Johnson");
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Name",
                keyColumnType: "nvarchar(max)",
                keyValue: "Bob Smith");
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Name",
                keyColumnType: "nvarchar(max)",
                keyValue: "Carol White");
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Name",
                keyColumnType: "nvarchar(max)",
                keyValue: "David Brown");
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Name",
                keyColumnType: "nvarchar(max)",
                keyValue: "Eva Martinez");
        }
    }
}
