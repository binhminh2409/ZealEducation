using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZealEducation.Migrations
{
    public partial class updateResource2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Resource",
                newName: "FileName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a3e0343-3171-4a8d-85be-7f45001e2284");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ac433250-2632-4c0d-a065-99f5d8913aaa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eca9395e-317e-45cb-bfbc-b2b8c5a03fda");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Resource",
                newName: "FilePath");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6dfe90ec-5a93-43fc-b303-d3a5794e4e4f", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7d078dc5-9cbf-495a-b5b9-77335515bc06", "2", "Faculty", "Faculty" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "82fa7bbe-7045-44d3-9c23-58f031eaa316", "3", "Candidate", "Candidate" });
        }
    }
}
