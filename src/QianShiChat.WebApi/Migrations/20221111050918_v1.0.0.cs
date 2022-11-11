using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QianShiChat.WebApi.Migrations
{
    public partial class v100 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "用户表主键")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Account = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "账号")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NickName = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "昵称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "密码")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false, comment: "是否已删除")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserRealtion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "用户关系表主键")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "用户编号"),
                    FriendId = table.Column<int>(type: "int", nullable: false, comment: "朋友编号"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRealtion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRealtion_UserInfo_FriendId",
                        column: x => x.FriendId,
                        principalTable: "UserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRealtion_UserInfo_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "UserInfo",
                columns: new[] { "Id", "Account", "NickName", "Password" },
                values: new object[] { 1, "admin", "Admin", "E10ADC3949BA59ABBE56E057F20F883E" });

            migrationBuilder.InsertData(
                table: "UserInfo",
                columns: new[] { "Id", "Account", "NickName", "Password" },
                values: new object[] { 2, "qianshi", "千矢", "E10ADC3949BA59ABBE56E057F20F883E" });

            migrationBuilder.InsertData(
                table: "UserInfo",
                columns: new[] { "Id", "Account", "NickName", "Password" },
                values: new object[] { 3, "kuriyama", "栗山未来", "E10ADC3949BA59ABBE56E057F20F883E" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRealtion_FriendId",
                table: "UserRealtion",
                column: "FriendId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRealtion_Id",
                table: "UserRealtion",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserRealtion_UserId",
                table: "UserRealtion",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRealtion");

            migrationBuilder.DropTable(
                name: "UserInfo");
        }
    }
}
