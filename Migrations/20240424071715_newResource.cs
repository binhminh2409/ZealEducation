using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZealEducation.Migrations
{
    public partial class newResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "Resource",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6dfe90ec-5a93-43fc-b303-d3a5794e4e4f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7d078dc5-9cbf-495a-b5b9-77335515bc06");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82fa7bbe-7045-44d3-9c23-58f031eaa316");

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "Resource",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "186a251d-b050-47db-b673-9166d3b2e6d6", "3", "Candidate", "Candidate" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a3890fde-a1cf-46d8-8698-1664f315adc1", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fd668acc-8460-40c2-9700-e000336545ce", "2", "Faculty", "Faculty" });
        }
    }
}
