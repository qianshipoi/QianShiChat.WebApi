using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QianShiChat.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class V106 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DefaultAvatar",
                table: "DefaultAvatar");

            migrationBuilder.RenameTable(
                name: "DefaultAvatar",
                newName: "DefaultAvatars");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DefaultAvatars",
                table: "DefaultAvatars",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DefaultAvatars",
                table: "DefaultAvatars");

            migrationBuilder.RenameTable(
                name: "DefaultAvatars",
                newName: "DefaultAvatar");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DefaultAvatar",
                table: "DefaultAvatar",
                column: "Id");
        }
    }
}
