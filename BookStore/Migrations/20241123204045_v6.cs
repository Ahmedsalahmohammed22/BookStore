using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStore.Migrations
{
    /// <inheritdoc />
    public partial class v6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "035f9668-a5dd-49b2-9c97-982eeaea464f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "916ec482-1e75-4891-9322-a3b35e6b46cc");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "035f9668-a5dd-49b2-9c97-982eeaea464f", null, "admin", "ADMIN" },
                    { "916ec482-1e75-4891-9322-a3b35e6b46cc", null, "customer", "CUSTOMER" }
                });
        }
    }
}
