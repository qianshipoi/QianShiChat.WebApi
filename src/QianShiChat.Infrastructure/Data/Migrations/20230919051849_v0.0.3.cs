using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QianShiChat.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class v003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessage_UserInfo_FromUserId",
                table: "ChatMessage");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessage_FromUserId",
                table: "ChatMessage");

            migrationBuilder.DropColumn(
                name: "FromUserId",
                table: "ChatMessage");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_FromId",
                table: "ChatMessage",
                column: "FromId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessage_UserInfo_FromId",
                table: "ChatMessage",
                column: "FromId",
                principalTable: "UserInfo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessage_UserInfo_FromId",
                table: "ChatMessage");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessage_FromId",
                table: "ChatMessage");

            migrationBuilder.AddColumn<int>(
                name: "FromUserId",
                table: "ChatMessage",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_FromUserId",
                table: "ChatMessage",
                column: "FromUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessage_UserInfo_FromUserId",
                table: "ChatMessage",
                column: "FromUserId",
                principalTable: "UserInfo",
                principalColumn: "Id");
        }
    }
}
