using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QianShiChat.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class v104 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "MessageCursor",
                type: "int",
                nullable: false,
                comment: "user id",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "user id")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageCursor_UserInfo_UserId",
                table: "MessageCursor",
                column: "UserId",
                principalTable: "UserInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageCursor_UserInfo_UserId",
                table: "MessageCursor");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "MessageCursor",
                type: "int",
                nullable: false,
                comment: "user id",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "user id")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);
        }
    }
}