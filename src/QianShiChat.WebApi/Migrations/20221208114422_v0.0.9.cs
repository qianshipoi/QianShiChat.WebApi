using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QianShiChat.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class v009 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Path",
                value: "/Raw/DefaultAvatar/1.jpg");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Path",
                value: "/Raw/DefaultAvatar/2.jpg");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Path",
                value: "/Raw/DefaultAvatar/3.jpg");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Path",
                value: "/Raw/DefaultAvatar/4.jpg");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 5L,
                column: "Path",
                value: "/Raw/DefaultAvatar/5.jpg");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Path",
                value: "/Raw/DefaultAvatar/6.jpg");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 7L,
                column: "Path",
                value: "/Raw/DefaultAvatar/7.jpg");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 8L,
                column: "Path",
                value: "/Raw/DefaultAvatar/8.jpg");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 9L,
                column: "Path",
                value: "/Raw/DefaultAvatar/9.jpg");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 10L,
                column: "Path",
                value: "/Raw/DefaultAvatar/10.jpg");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 11L,
                column: "Path",
                value: "/Raw/DefaultAvatar/11.png");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 12L,
                column: "Path",
                value: "/Raw/DefaultAvatar/12.gif");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 13L,
                column: "Path",
                value: "/Raw/DefaultAvatar/13.gif");

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Path",
                value: "/Raw/DefaultAvatar/1.jpg");

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Path",
                value: "/Raw/DefaultAvatar/2.jpg");

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Path",
                value: "/Raw/DefaultAvatar/3.jpg");

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Path",
                value: "/Raw/DefaultAvatar/4.jpg");

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 5L,
                column: "Path",
                value: "/Raw/DefaultAvatar/5.jpg");

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Path",
                value: "/Raw/DefaultAvatar/6.jpg");

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 7L,
                column: "Path",
                value: "/Raw/DefaultAvatar/7.jpg");

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 8L,
                column: "Path",
                value: "/Raw/DefaultAvatar/8.jpg");

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 9L,
                column: "Path",
                value: "/Raw/DefaultAvatar/9.jpg");

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 1,
                column: "Avatar",
                value: "/Raw/DefaultAvatar/1.jpg");

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 2,
                column: "Avatar",
                value: "/Raw/DefaultAvatar/2.jpg");

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 3,
                column: "Avatar",
                value: "/Raw/DefaultAvatar/3.jpg");

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 4,
                column: "Avatar",
                value: "/Raw/DefaultAvatar/4.jpg");

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 5,
                column: "Avatar",
                value: "/Raw/DefaultAvatar/5.jpg");

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 6,
                column: "Avatar",
                value: "/Raw/DefaultAvatar/6.jpg");

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 7,
                column: "Avatar",
                value: "/Raw/DefaultAvatar/7.jpg");

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 8,
                column: "Avatar",
                value: "/Raw/DefaultAvatar/8.jpg");

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 9,
                column: "Avatar",
                value: "/Raw/DefaultAvatar/9.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Path",
                value: "/raw/DefaultAvatar/1.jpg");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Path",
                value: "/raw/DefaultAvatar/2.jpg");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Path",
                value: "/raw/DefaultAvatar/3.jpg");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Path",
                value: "/raw/DefaultAvatar/4.jpg");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 5L,
                column: "Path",
                value: "/raw/DefaultAvatar/5.jpg");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Path",
                value: "/raw/DefaultAvatar/6.jpg");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 7L,
                column: "Path",
                value: "/raw/DefaultAvatar/7.jpg");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 8L,
                column: "Path",
                value: "/raw/DefaultAvatar/8.jpg");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 9L,
                column: "Path",
                value: "/raw/DefaultAvatar/9.jpg");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 10L,
                column: "Path",
                value: "/raw/DefaultAvatar/10.jpg");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 11L,
                column: "Path",
                value: "/raw/DefaultAvatar/11.png");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 12L,
                column: "Path",
                value: "/raw/DefaultAvatar/12.gif");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 13L,
                column: "Path",
                value: "/raw/DefaultAvatar/13.gif");

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Path",
                value: "/raw/DefaultAvatar/1.jpg");

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Path",
                value: "/raw/DefaultAvatar/2.jpg");

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Path",
                value: "/raw/DefaultAvatar/3.jpg");

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Path",
                value: "/raw/DefaultAvatar/4.jpg");

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 5L,
                column: "Path",
                value: "/raw/DefaultAvatar/5.jpg");

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Path",
                value: "/raw/DefaultAvatar/6.jpg");

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 7L,
                column: "Path",
                value: "/raw/DefaultAvatar/7.jpg");

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 8L,
                column: "Path",
                value: "/raw/DefaultAvatar/8.jpg");

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 9L,
                column: "Path",
                value: "/raw/DefaultAvatar/9.jpg");

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 1,
                column: "Avatar",
                value: "/raw/DefaultAvatar/1.jpg");

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 2,
                column: "Avatar",
                value: "/raw/DefaultAvatar/2.jpg");

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 3,
                column: "Avatar",
                value: "/raw/DefaultAvatar/3.jpg");

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 4,
                column: "Avatar",
                value: "/raw/DefaultAvatar/4.jpg");

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 5,
                column: "Avatar",
                value: "/raw/DefaultAvatar/5.jpg");

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 6,
                column: "Avatar",
                value: "/raw/DefaultAvatar/6.jpg");

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 7,
                column: "Avatar",
                value: "/raw/DefaultAvatar/7.jpg");

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 8,
                column: "Avatar",
                value: "/raw/DefaultAvatar/8.jpg");

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 9,
                column: "Avatar",
                value: "/raw/DefaultAvatar/9.jpg");
        }
    }
}
