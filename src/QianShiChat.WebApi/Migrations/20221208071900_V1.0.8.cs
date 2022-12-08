using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QianShiChat.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class V108 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DefaultAvatars",
                columns: new[] { "Id", "CreateTime", "DeleteTime", "IsDeleted", "Path", "Size" },
                values: new object[,]
                {
                    { 10L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/10.jpg", 113664ul },
                    { 11L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/11.png", 679936ul },
                    { 12L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/12.gif", 900096ul },
                    { 13L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/13.gif", 748544ul }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 13L);
        }
    }
}
