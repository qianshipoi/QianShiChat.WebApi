using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QianShiChat.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class v004 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "UserRealtion",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "UserGroupRealtion",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "UserRealtion",
                keyColumn: "Id",
                keyValue: 1,
                column: "Alias",
                value: null);

            migrationBuilder.UpdateData(
                table: "UserRealtion",
                keyColumn: "Id",
                keyValue: 2,
                column: "Alias",
                value: null);

            migrationBuilder.UpdateData(
                table: "UserRealtion",
                keyColumn: "Id",
                keyValue: 3,
                column: "Alias",
                value: null);

            migrationBuilder.UpdateData(
                table: "UserRealtion",
                keyColumn: "Id",
                keyValue: 4,
                column: "Alias",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alias",
                table: "UserRealtion");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "UserGroupRealtion");
        }
    }
}
