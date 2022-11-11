using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QianShiChat.WebApi.Migrations
{
    public partial class v101 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FriendApply",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "申请编号")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FriendId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<sbyte>(type: "tinyint", nullable: false),
                    LaseUpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendApply");
        }
    }
}
