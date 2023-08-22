﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QianShiChat.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class v001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DefaultAvatars",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "avatar id.")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<long>(type: "bigint", nullable: false, comment: "create time."),
                    Path = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "file path.")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Size = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "file size."),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "is deleted."),
                    DeleteTime = table.Column<long>(type: "bigint", nullable: false, comment: "delete time."),
                    UpdateTime = table.Column<long>(type: "bigint", nullable: false, comment: "update time.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultAvatars", x => x.Id);
                })
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
                    Description = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true, comment: "描述")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<long>(type: "bigint", nullable: false, comment: "创建时间"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false, comment: "是否已删除"),
                    UpdateTime = table.Column<long>(type: "bigint", nullable: false, comment: "修改时间"),
                    DeleteTime = table.Column<long>(type: "bigint", nullable: false, comment: "删除时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RawPath = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PreviewPath = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Hash = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContentType = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DeleteTime = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<long>(type: "bigint", nullable: false),
                    UpdateTime = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_UserInfo_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChatMessage",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FromId = table.Column<int>(type: "int", nullable: false),
                    ToId = table.Column<int>(type: "int", nullable: false),
                    SessionId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SendType = table.Column<sbyte>(type: "tinyint", nullable: false),
                    MessageType = table.Column<sbyte>(type: "tinyint", nullable: false),
                    Content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<long>(type: "bigint", nullable: false),
                    UpdateTime = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessage_UserInfo_FromId",
                        column: x => x.FromId,
                        principalTable: "UserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FriendApply",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FriendId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<sbyte>(type: "tinyint", nullable: false),
                    Remark = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdateTime = table.Column<long>(type: "bigint", nullable: false)
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
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalUser = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeleteTime = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_UserInfo_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MessageCursor",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "user id"),
                    Postiton = table.Column<long>(type: "bigint", nullable: false, comment: "message position"),
                    LastUpdateTime = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageCursor", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_MessageCursor_UserInfo_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    UpdateTime = table.Column<long>(type: "bigint", nullable: false, comment: "update time."),
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

            migrationBuilder.CreateTable(
                name: "GroupApplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<sbyte>(type: "tinyint", nullable: false),
                    Remark = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdateTime = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupApplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupApplies_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupApplies_UserInfo_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserGroupRealtions",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroupRealtions", x => new { x.UserId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_UserGroupRealtions_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroupRealtions_UserInfo_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "DefaultAvatars",
                columns: new[] { "Id", "CreateTime", "DeleteTime", "IsDeleted", "Path", "Size", "UpdateTime" },
                values: new object[,]
                {
                    { 1L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/1.jpg", 208896ul, 0L },
                    { 2L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/2.jpg", 30720ul, 0L },
                    { 3L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/3.jpg", 87757ul, 0L },
                    { 4L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/4.jpg", 159744ul, 0L },
                    { 5L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/5.jpg", 73216ul, 0L },
                    { 6L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/6.jpg", 108544ul, 0L },
                    { 7L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/7.jpg", 93799ul, 0L },
                    { 8L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/8.jpg", 112640ul, 0L },
                    { 9L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/9.jpg", 108544ul, 0L },
                    { 10L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/10.jpg", 113664ul, 0L },
                    { 11L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/11.png", 679936ul, 0L },
                    { 12L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/12.gif", 900096ul, 0L },
                    { 13L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/13.gif", 748544ul, 0L }
                });

            migrationBuilder.InsertData(
                table: "UserInfo",
                columns: new[] { "Id", "Account", "Avatar", "CreateTime", "DeleteTime", "Description", "NickName", "Password", "UpdateTime" },
                values: new object[,]
                {
                    { 1, "admin", "/Raw/DefaultAvatar/1.jpg", 1668583992424L, 0L, "炒鸡管理员！", "Admin", "E10ADC3949BA59ABBE56E057F20F883E", 0L },
                    { 2, "qianshi", "/Raw/DefaultAvatar/2.jpg", 1668583992424L, 0L, "道歉的时候露出肚皮才是常识！", "千矢", "E10ADC3949BA59ABBE56E057F20F883E", 0L },
                    { 3, "kuriyama", "/Raw/DefaultAvatar/3.jpg", 1668583992424L, 0L, "不愉快です", "栗山未来", "E10ADC3949BA59ABBE56E057F20F883E", 0L },
                    { 4, "admin1", "/Raw/DefaultAvatar/4.jpg", 1668583992424L, 0L, null, "Admin1", "E10ADC3949BA59ABBE56E057F20F883E", 0L },
                    { 5, "qianshi1", "/Raw/DefaultAvatar/5.jpg", 1668583992424L, 0L, null, "千矢1", "E10ADC3949BA59ABBE56E057F20F883E", 0L },
                    { 6, "kuriyama1", "/Raw/DefaultAvatar/6.jpg", 1668583992424L, 0L, null, "栗山未来1", "E10ADC3949BA59ABBE56E057F20F883E", 0L },
                    { 7, "admin2", "/Raw/DefaultAvatar/7.jpg", 1668583992424L, 0L, null, "Admin2", "E10ADC3949BA59ABBE56E057F20F883E", 0L },
                    { 8, "qianshi2", "/Raw/DefaultAvatar/8.jpg", 1668583992424L, 0L, null, "千矢2", "E10ADC3949BA59ABBE56E057F20F883E", 0L },
                    { 9, "kuriyama2", "/Raw/DefaultAvatar/9.jpg", 1668583992424L, 0L, null, "栗山未来2", "E10ADC3949BA59ABBE56E057F20F883E", 0L }
                });

            migrationBuilder.InsertData(
                table: "FriendApply",
                columns: new[] { "Id", "CreateTime", "FriendId", "Remark", "Status", "UpdateTime", "UserId" },
                values: new object[,]
                {
                    { 1, 1668583992424L, 2, "很高兴认识你。", (sbyte)3, 1668583992424L, 1 },
                    { 2, 1668583992424L, 2, "Nice to meet you.", (sbyte)2, 1668583992424L, 1 },
                    { 3, 1668583992424L, 3, "hi!", (sbyte)1, 1668583992424L, 1 }
                });

            migrationBuilder.InsertData(
                table: "UserAvatars",
                columns: new[] { "Id", "CreateTime", "DeleteTime", "IsDeleted", "Path", "Size", "UpdateTime", "UserId" },
                values: new object[,]
                {
                    { 1L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/1.jpg", 208896ul, 0L, 1 },
                    { 2L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/2.jpg", 30720ul, 0L, 2 },
                    { 3L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/3.jpg", 87757ul, 0L, 3 },
                    { 4L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/4.jpg", 159744ul, 0L, 4 },
                    { 5L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/5.jpg", 73216ul, 0L, 5 },
                    { 6L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/6.jpg", 108544ul, 0L, 6 },
                    { 7L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/7.jpg", 93799ul, 0L, 7 },
                    { 8L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/8.jpg", 112640ul, 0L, 8 },
                    { 9L, 1668583992424L, 0L, false, "/Raw/DefaultAvatar/9.jpg", 108544ul, 0L, 9 }
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
                name: "IX_Attachments_Hash",
                table: "Attachments",
                column: "Hash");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_UserId",
                table: "Attachments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_FromId",
                table: "ChatMessage",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendApply_FriendId",
                table: "FriendApply",
                column: "FriendId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendApply_UserId",
                table: "FriendApply",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupApplies_GroupId",
                table: "GroupApplies",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupApplies_UserId",
                table: "GroupApplies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_UserId",
                table: "Groups",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAvatars_UserId",
                table: "UserAvatars",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRealtions_GroupId",
                table: "UserGroupRealtions",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRealtion_FriendId",
                table: "UserRealtion",
                column: "FriendId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRealtion_UserId",
                table: "UserRealtion",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "ChatMessage");

            migrationBuilder.DropTable(
                name: "DefaultAvatars");

            migrationBuilder.DropTable(
                name: "FriendApply");

            migrationBuilder.DropTable(
                name: "GroupApplies");

            migrationBuilder.DropTable(
                name: "MessageCursor");

            migrationBuilder.DropTable(
                name: "UserAvatars");

            migrationBuilder.DropTable(
                name: "UserGroupRealtions");

            migrationBuilder.DropTable(
                name: "UserRealtion");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "UserInfo");
        }
    }
}
