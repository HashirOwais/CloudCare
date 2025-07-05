using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CloudCare.API.Migrations
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRecurring = table.Column<bool>(type: "boolean", nullable: false),
                    ReceiptUrl = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    VendorId = table.Column<int>(type: "integer", nullable: false),
                    PaymentMethodId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expenses_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expenses_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Food & Snacks" },
                    { 2, "Educational Supplies" },
                    { 3, "Toys & Games" },
                    { 4, "Cleaning Supplies" },
                    { 5, "Utilities" },
                    { 6, "Office Supplies" },
                    { 7, "Furniture & Fixtures" },
                    { 8, "Repairs & Maintenance" },
                    { 9, "Transportation" },
                    { 10, "Insurance" },
                    { 11, "Professional Services" },
                    { 12, "Marketing & Advertising" },
                    { 13, "Staff Wages" },
                    { 14, "Training & Development" },
                    { 15, "Licenses & Permits" },
                    { 99, "Miscellaneous" }
                });

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Credit Card" },
                    { 2, "Debit Card" },
                    { 3, "Cash" },
                    { 4, "E-Transfer" },
                    { 5, "Cheque" }
                });

            migrationBuilder.InsertData(
                table: "Vendors",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Walmart" },
                    { 2, "Amazon" },
                    { 3, "Costco" },
                    { 4, "Staples" },
                    { 5, "Home Depot" },
                    { 6, "Best Buy" },
                    { 7, "Private Marketplace" },
                    { 8, "Local Vendor" },
                    { 9, "Government" },
                    { 99, "Miscellaneous" }
                });

            migrationBuilder.InsertData(
                table: "Expenses",
                columns: new[] { "Id", "Amount", "CategoryId", "Date", "Description", "IsRecurring", "Notes", "PaymentMethodId", "ReceiptUrl", "UserId", "VendorId" },
                values: new object[,]
                {
                    { 1, 20.00m, 1, new DateTime(2024, 7, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Snacks for kids", false, null, 1, null, 1, 1 },
                    { 2, 35.00m, 3, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Toys purchase", true, null, 2, null, 1, 2 },
                    { 3, 50.00m, 2, new DateTime(2024, 6, 29, 0, 0, 0, 0, DateTimeKind.Utc), "Field Trip Supplies", false, null, 1, null, 1, 3 },
                    { 4, 22.50m, 6, new DateTime(2024, 6, 28, 0, 0, 0, 0, DateTimeKind.Utc), "Office Supplies restock", false, null, 1, null, 1, 1 },
                    { 5, 80.00m, 2, new DateTime(2024, 6, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Books for reading time", false, null, 2, null, 1, 2 },
                    { 6, 120.00m, 4, new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), "Monthly Cleaning Service", true, null, 3, null, 1, 3 },
                    { 7, 150.00m, 1, new DateTime(2024, 6, 19, 0, 0, 0, 0, DateTimeKind.Utc), "Birthday Party expenses", false, null, 1, null, 1, 1 },
                    { 8, 60.00m, 6, new DateTime(2024, 6, 14, 0, 0, 0, 0, DateTimeKind.Utc), "Printer Ink replacement", false, null, 2, null, 1, 2 },
                    { 9, 95.00m, 14, new DateTime(2024, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), "Online Workshop for Staff", false, null, 1, null, 1, 3 },
                    { 10, 25.00m, 1, new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Weekly Snacks", true, null, 3, null, 1, 1 },
                    { 11, 45.99m, 12, new DateTime(2024, 6, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Monthly Software Subscription", true, null, 2, null, 1, 2 },
                    { 12, 15.00m, 2, new DateTime(2024, 7, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Art Supplies", false, null, 1, null, 2, 1 },
                    { 13, 45.00m, 1, new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lunch for Staff", false, null, 3, null, 2, 2 },
                    { 14, 120.00m, 4, new DateTime(2024, 6, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Monthly Cleaning Service", true, null, 2, null, 2, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CategoryId",
                table: "Expenses",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_PaymentMethodId",
                table: "Expenses",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_VendorId",
                table: "Expenses",
                column: "VendorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "Vendors");
        }
    }
}
