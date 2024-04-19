using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZealEducation.Migrations
{
    public partial class updateDb3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Submission",
                newName: "FileName");

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a0b4186-afc9-4517-93e5-610a6e98a483");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6cd095fe-c6bc-4bc7-b9b6-9c6d69782193");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f987cfbd-bf91-4239-a07c-1aa7d236d4a3");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Submission",
                newName: "FilePath");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7562364d-4896-48e6-a7fd-f769642a0f1d", "3", "Candidate", "Candidate" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "897e6981-e7a6-4add-bdd5-112f8b8d5b39", "2", "Faculty", "Faculty" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "eb1b370b-0576-486d-aaf1-a518507ebaf2", "1", "Admin", "Admin" });
        }
    }
}
