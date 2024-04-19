using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZealEducation.Migrations
{
    public partial class UpdateExamDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Result");

            

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Submission",
                newName: "SubmitDateTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "MarkedDateTime",
                table: "Submission",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Passed",
                table: "Submission",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Submission",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Exams",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScoreToPass",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6afd9498-1c3c-4113-9fcd-318eb83cc17b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a301a6ed-1186-4555-bbca-0d56883255ea");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "beee50a8-673f-4f8c-9c6d-b4308d5e8e18");

            migrationBuilder.DropColumn(
                name: "MarkedDateTime",
                table: "Submission");

            migrationBuilder.DropColumn(
                name: "Passed",
                table: "Submission");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Submission");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "ScoreToPass",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "SubmitDateTime",
                table: "Submission",
                newName: "DateTime");

            migrationBuilder.CreateTable(
                name: "Result",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubmissionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Passed = table.Column<bool>(type: "bit", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Result", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Result_Submission_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "69bf3197-49ae-4f46-b8b6-91a622953d7f", "2", "Faculty", "Faculty" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6ad20553-e758-4d58-b29c-f519b1c75805", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ab0b21f4-bf3e-4e08-9bba-1589208b9673", "3", "Candidate", "Candidate" });

            migrationBuilder.CreateIndex(
                name: "IX_Result_SubmissionId",
                table: "Result",
                column: "SubmissionId");
        }
    }
}
