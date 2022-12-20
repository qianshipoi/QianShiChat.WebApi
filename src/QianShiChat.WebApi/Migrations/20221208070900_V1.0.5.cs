using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QianShiChat.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class V105 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DefaultAvatar",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "avatar id.")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<long>(type: "bigint", nullable: false, comment: "create time."),
                    Path = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "file path.")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Size = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "file size."),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "is deleted."),
                    DeleteTime = table.Column<long>(type: "bigint", nullable: false, comment: "delete time.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultAvatar", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserAvatars",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "avatar id.")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "user id."),
                    CreateTime = table.Column<long>(type: "bigint", nullable: false, comment: "create time."),
                    Path = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "file path.")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Size = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "file size."),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "is deleted."),
                    DeleteTime = table.Column<long>(type: "bigint", nullable: false, comment: "delete time.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAvatars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAvatars_UserInfo_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "DefaultAvatar",
                columns: new[] { "Id", "CreateTime", "DeleteTime", "IsDeleted", "Path", "Size" },
                values: new object[,]
                {
                    { 1L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/1.jpg", 208896ul },
                    { 2L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/2.jpg", 30720ul },
                    { 3L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/3.jpg", 87757ul },
                    { 4L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/4.jpg", 159744ul },
                    { 5L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/5.jpg", 73216ul },
                    { 6L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/6.jpg", 108544ul },
                    { 7L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/7.jpg", 93799ul },
                    { 8L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/8.jpg", 112640ul },
                    { 9L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/9.jpg", 108544ul }
                });

            migrationBuilder.InsertData(
                table: "UserAvatars",
                columns: new[] { "Id", "CreateTime", "DeleteTime", "IsDeleted", "Path", "Size", "UserId" },
                values: new object[,]
                {
                    { 1L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/1.jpg", 208896ul, 1 },
                    { 2L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/2.jpg", 30720ul, 1 },
                    { 3L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/3.jpg", 87757ul, 1 },
                    { 4L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/4.jpg", 159744ul, 1 },
                    { 5L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/5.jpg", 73216ul, 1 },
                    { 6L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/6.jpg", 108544ul, 1 },
                    { 7L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/7.jpg", 93799ul, 1 },
                    { 8L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/8.jpg", 112640ul, 1 },
                    { 9L, 1668583992424L, 0L, false, "/raw/DefaultAvatar/9.jpg", 108544ul, 1 }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_UserAvatars_UserId",
                table: "UserAvatars",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DefaultAvatar");

            migrationBuilder.DropTable(
                name: "UserAvatars");

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 1,
                column: "Avatar",
                value: null);

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 2,
                column: "Avatar",
                value: null);

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 3,
                column: "Avatar",
                value: null);

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 4,
                column: "Avatar",
                value: null);

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 5,
                column: "Avatar",
                value: null);

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 6,
                column: "Avatar",
                value: null);

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 7,
                column: "Avatar",
                value: null);

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 8,
                column: "Avatar",
                value: null);

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 9,
                column: "Avatar",
                value: null);
        }
    }
}