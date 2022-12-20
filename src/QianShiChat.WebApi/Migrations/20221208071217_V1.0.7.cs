using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QianShiChat.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class V107 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 2L,
                column: "UserId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 3L,
                column: "UserId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 4L,
                column: "UserId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 5L,
                column: "UserId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 6L,
                column: "UserId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 7L,
                column: "UserId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 8L,
                column: "UserId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 9L,
                column: "UserId",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 2L,
                column: "UserId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 3L,
                column: "UserId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 4L,
                column: "UserId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 5L,
                column: "UserId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 6L,
                column: "UserId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 7L,
                column: "UserId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 8L,
                column: "UserId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 9L,
                column: "UserId",
                value: 1);
        }
    }
}