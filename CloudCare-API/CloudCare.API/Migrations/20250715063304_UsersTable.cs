using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CloudCare.API.Migrations
{
    /// <inheritdoc />
    public partial class UsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DaycareName = table.Column<string>(type: "text", nullable: false),
                    DaycareAddress = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    WebsiteUrl = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    UserCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DaycareAddress", "DaycareName", "Email", "Name", "Notes", "PhoneNumber", "UserCreated", "WebsiteUrl" },
                values: new object[,]
                {
                    { 1, "123 Main St, Cityville", "Happy Kids Daycare", "provider1@daycare.com", "Alice Johnson", "Open weekdays 7am-6pm", "555-1234", new DateTime(2024, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), "https://happykidsdaycare.com" },
                    { 2, "456 Oak Ave, Townsville", "Little Stars Childcare", "provider2@daycare.com", "Bob Smith", "Accepts infants and toddlers", "555-5678", new DateTime(2024, 6, 2, 9, 0, 0, 0, DateTimeKind.Utc), "https://littlestarschildcare.com" },
                    { 3, "789 Pine Rd, Villagetown", "Bright Minds Preschool", "provider3@daycare.com", "Carol Lee", "Focus on early learning", "555-9012", new DateTime(2024, 6, 3, 10, 0, 0, 0, DateTimeKind.Utc), "https://brightmindspreschool.com" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_UserId",
                table: "Expenses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Users_UserId",
                table: "Expenses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users_UserId",
                table: "Expenses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_UserId",
                table: "Expenses");
        }
    }
}
