using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QianShiChat.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class v109 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LaseUpdateTime",
                table: "FriendApply",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "LastUpdateTime",
                table: "ChatMessage",
                newName: "UpdateTime");

            migrationBuilder.AddColumn<long>(
                name: "DeleteTime",
                table: "UserInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "删除时间");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "UserInfo",
                type: "varchar(128)",
                maxLength: 128,
                nullable: true,
                comment: "描述")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "UpdateTime",
                table: "UserInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "修改时间");

            migrationBuilder.AddColumn<long>(
                name: "UpdateTime",
                table: "UserAvatars",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "update time.");

            migrationBuilder.AddColumn<long>(
                name: "UpdateTime",
                table: "DefaultAvatars",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "update time.");

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 1L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 2L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 3L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 4L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 5L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 6L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 7L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 8L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 9L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 10L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 11L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 12L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "DefaultAvatars",
                keyColumn: "Id",
                keyValue: 13L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 1L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 2L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 3L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 4L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 5L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 6L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 7L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 8L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "UserAvatars",
                keyColumn: "Id",
                keyValue: 9L,
                column: "UpdateTime",
                value: 0L);

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DeleteTime", "Description", "UpdateTime" },
                values: new object[] { 0L, "炒鸡管理员！", 0L });

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DeleteTime", "Description", "UpdateTime" },
                values: new object[] { 0L, "道歉的时候露出肚皮才是常识！", 0L });

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DeleteTime", "Description", "UpdateTime" },
                values: new object[] { 0L, "不愉快です", 0L });

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DeleteTime", "Description", "UpdateTime" },
                values: new object[] { 0L, null, 0L });

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DeleteTime", "Description", "UpdateTime" },
                values: new object[] { 0L, null, 0L });

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DeleteTime", "Description", "UpdateTime" },
                values: new object[] { 0L, null, 0L });

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DeleteTime", "Description", "UpdateTime" },
                values: new object[] { 0L, null, 0L });

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DeleteTime", "Description", "UpdateTime" },
                values: new object[] { 0L, null, 0L });

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DeleteTime", "Description", "UpdateTime" },
                values: new object[] { 0L, null, 0L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteTime",
                table: "UserInfo");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "UserInfo");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "UserInfo");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "UserAvatars");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "DefaultAvatars");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "FriendApply",
                newName: "LaseUpdateTime");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "ChatMessage",
                newName: "LastUpdateTime");
        }
    }
}