using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QianShiChat.WebApi.Migrations
{
    public partial class v102 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FormId",
                table: "ChatMessage",
                newName: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_FromId",
                table: "ChatMessage",
                column: "FromId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessage_UserInfo_FromId",
                table: "ChatMessage",
                column: "FromId",
                principalTable: "UserInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessage_UserInfo_FromId",
                table: "ChatMessage");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessage_FromId",
                table: "ChatMessage");

            migrationBuilder.RenameColumn(
                name: "FromId",
                table: "ChatMessage",
                newName: "FormId");
        }
    }
}
