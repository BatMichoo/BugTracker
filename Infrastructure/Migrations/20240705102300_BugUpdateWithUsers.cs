using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class BugUpdateWithUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssigneeId",
                table: "Bugs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Bugs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedById",
                table: "Bugs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Bugs_AssigneeId",
                table: "Bugs",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Bugs_CreatorId",
                table: "Bugs",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Bugs_LastUpdatedById",
                table: "Bugs",
                column: "LastUpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Bugs_AspNetUsers_AssigneeId",
                table: "Bugs",
                column: "AssigneeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bugs_AspNetUsers_CreatorId",
                table: "Bugs",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bugs_AspNetUsers_LastUpdatedById",
                table: "Bugs",
                column: "LastUpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bugs_AspNetUsers_AssigneeId",
                table: "Bugs");

            migrationBuilder.DropForeignKey(
                name: "FK_Bugs_AspNetUsers_CreatorId",
                table: "Bugs");

            migrationBuilder.DropForeignKey(
                name: "FK_Bugs_AspNetUsers_LastUpdatedById",
                table: "Bugs");

            migrationBuilder.DropIndex(
                name: "IX_Bugs_AssigneeId",
                table: "Bugs");

            migrationBuilder.DropIndex(
                name: "IX_Bugs_CreatorId",
                table: "Bugs");

            migrationBuilder.DropIndex(
                name: "IX_Bugs_LastUpdatedById",
                table: "Bugs");

            migrationBuilder.DropColumn(
                name: "AssigneeId",
                table: "Bugs");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Bugs");

            migrationBuilder.DropColumn(
                name: "LastUpdatedById",
                table: "Bugs");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");
        }
    }
}
