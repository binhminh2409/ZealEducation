using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZealEducation.Migrations
{
    public partial class RoleSeeded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1baeed92-1240-464d-9f15-d30e16bd7c1f", "2", "Faculty", "Faculty" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "27a91462-7129-4f76-a922-206e4cfd40ab", "3", "Candidate", "Candidate" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "65c853be-6d47-47f3-a2a9-1660630c3f9e", "1", "Admin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1baeed92-1240-464d-9f15-d30e16bd7c1f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "27a91462-7129-4f76-a922-206e4cfd40ab");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "65c853be-6d47-47f3-a2a9-1660630c3f9e");
        }
    }
}
