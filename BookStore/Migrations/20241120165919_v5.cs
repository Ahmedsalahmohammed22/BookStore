using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStore.Migrations
{
    /// <inheritdoc />
    public partial class v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3782a32b-2660-4dc9-9f25-1ae6e4e9b227");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d8da2258-d655-417c-ba28-0b638e05bb95");

            migrationBuilder.AddColumn<string>(
                name: "photoPath",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "035f9668-a5dd-49b2-9c97-982eeaea464f", null, "admin", "ADMIN" },
                    { "916ec482-1e75-4891-9322-a3b35e6b46cc", null, "customer", "CUSTOMER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "035f9668-a5dd-49b2-9c97-982eeaea464f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "916ec482-1e75-4891-9322-a3b35e6b46cc");

            migrationBuilder.DropColumn(
                name: "photoPath",
                table: "Books");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3782a32b-2660-4dc9-9f25-1ae6e4e9b227", null, "admin", "ADMIN" },
                    { "d8da2258-d655-417c-ba28-0b638e05bb95", null, "customer", "CUSTOMER" }
                });
        }
    }
}
