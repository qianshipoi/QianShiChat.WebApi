using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QianShiChat.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class v005 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Groups",
                type: "varchar(512)",
                maxLength: 512,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "GroupApplies",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "DeleteTime",
                table: "GroupApplies",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "GroupApplies",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "FriendApply",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "DeleteTime",
                table: "FriendApply",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FriendApply",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "FriendApply",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DeleteTime", "IsDeleted" },
                values: new object[] { 0L, false });

            migrationBuilder.UpdateData(
                table: "FriendApply",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DeleteTime", "IsDeleted" },
                values: new object[] { 0L, false });

            migrationBuilder.UpdateData(
                table: "FriendApply",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DeleteTime", "IsDeleted" },
                values: new object[] { 0L, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "DeleteTime",
                table: "GroupApplies");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "GroupApplies");

            migrationBuilder.DropColumn(
                name: "DeleteTime",
                table: "FriendApply");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FriendApply");

            migrationBuilder.UpdateData(
                table: "GroupApplies",
                keyColumn: "Remark",
                keyValue: null,
                column: "Remark",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "GroupApplies",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "FriendApply",
                keyColumn: "Remark",
                keyValue: null,
                column: "Remark",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "FriendApply",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
