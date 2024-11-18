using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class BugDeleteFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Bugs_BugId",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Bugs_BugId",
                table: "Comments",
                column: "BugId",
                principalTable: "Bugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Bugs_BugId",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Bugs_BugId",
                table: "Comments",
                column: "BugId",
                principalTable: "Bugs",
                principalColumn: "Id");
        }
    }
}
