using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStore.Migrations
{
    /// <inheritdoc />
    public partial class v7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4ee22b6f-8fb9-4aa9-b765-689cc9f47528");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "781d8a64-c607-475a-92b1-9227bc51bac6");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Books");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7efdd7f8-2a13-4533-890a-16d826fa5e46", null, "customer", "CUSTOMER" },
                    { "91d1d5f1-10f2-48e2-8715-cd5f3f5ece4e", null, "admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7efdd7f8-2a13-4533-890a-16d826fa5e46");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "91d1d5f1-10f2-48e2-8715-cd5f3f5ece4e");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Books",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4ee22b6f-8fb9-4aa9-b765-689cc9f47528", null, "admin", "ADMIN" },
                    { "781d8a64-c607-475a-92b1-9227bc51bac6", null, "customer", "CUSTOMER" }
                });
        }
    }
}
