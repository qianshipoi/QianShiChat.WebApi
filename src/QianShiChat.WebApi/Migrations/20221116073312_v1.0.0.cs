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
                    Avatar = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<long>(type: "bigint", nullable: false, comment: "创建时间"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false, comment: "是否已删除")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FriendApply",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "申请编号")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FriendId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<sbyte>(type: "tinyint", nullable: false),
                    Remark = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LaseUpdateTime = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendApply", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendApply_UserInfo_FriendId",
                        column: x => x.FriendId,
                        principalTable: "UserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendApply_UserInfo_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    CreateTime = table.Column<long>(type: "bigint", nullable: false, comment: "创建时间")
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
                columns: new[] { "Id", "Account", "Avatar", "CreateTime", "NickName", "Password" },
                values: new object[,]
                {
                    { 1, "admin", null, 1668583992424L, "Admin", "E10ADC3949BA59ABBE56E057F20F883E" },
                    { 2, "qianshi", null, 1668583992424L, "千矢", "E10ADC3949BA59ABBE56E057F20F883E" },
                    { 3, "kuriyama", null, 1668583992424L, "栗山未来", "E10ADC3949BA59ABBE56E057F20F883E" },
                    { 4, "admin1", null, 1668583992424L, "Admin1", "E10ADC3949BA59ABBE56E057F20F883E" },
                    { 5, "qianshi1", null, 1668583992424L, "千矢1", "E10ADC3949BA59ABBE56E057F20F883E" },
                    { 6, "kuriyama1", null, 1668583992424L, "栗山未来1", "E10ADC3949BA59ABBE56E057F20F883E" },
                    { 7, "admin2", null, 1668583992424L, "Admin2", "E10ADC3949BA59ABBE56E057F20F883E" },
                    { 8, "qianshi2", null, 1668583992424L, "千矢2", "E10ADC3949BA59ABBE56E057F20F883E" },
                    { 9, "kuriyama2", null, 1668583992424L, "栗山未来2", "E10ADC3949BA59ABBE56E057F20F883E" }
                });

            migrationBuilder.InsertData(
                table: "FriendApply",
                columns: new[] { "Id", "CreateTime", "FriendId", "LaseUpdateTime", "Remark", "Status", "UserId" },
                values: new object[,]
                {
                    { 1, 1668583992424L, 2, 1668583992424L, "很高兴认识你。", (sbyte)3, 1 },
                    { 2, 1668583992424L, 2, 1668583992424L, "Nice to meet you.", (sbyte)2, 1 },
                    { 3, 1668583992424L, 3, 1668583992424L, "hi!", (sbyte)1, 1 }
                });

            migrationBuilder.InsertData(
                table: "UserRealtion",
                columns: new[] { "Id", "CreateTime", "FriendId", "UserId" },
                values: new object[,]
                {
                    { 1, 1668583992424L, 2, 1 },
                    { 2, 1668583992424L, 1, 2 },
                    { 3, 1668583992424L, 3, 2 },
                    { 4, 1668583992424L, 2, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendApply_FriendId",
                table: "FriendApply",
                column: "FriendId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendApply_Id",
                table: "FriendApply",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FriendApply_UserId",
                table: "FriendApply",
                column: "UserId");

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
                name: "FriendApply");

            migrationBuilder.DropTable(
                name: "UserRealtion");

            migrationBuilder.DropTable(
                name: "UserInfo");
        }
    }
}