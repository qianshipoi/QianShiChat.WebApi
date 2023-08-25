using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QianShiChat.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class v004 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Sessions",
                table: "Sessions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sessions",
                table: "Sessions",
                columns: new[] { "Id", "FromId" });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "FromId", "Id", "CreateTime", "DeleteTime", "IsDeleted", "MessagePosition", "ToId", "Type", "UpdateTime" },
                values: new object[,]
                {
                    { 1, "personal-1-2", 1668583992424L, 0L, false, 1668583992424L, 2, (sbyte)1, 1668583992424L },
                    { 2, "personal-1-2", 1668583992424L, 0L, false, 1668583992424L, 1, (sbyte)1, 1668583992424L },
                    { 2, "personal-2-3", 1668583992424L, 0L, false, 1668583992424L, 3, (sbyte)1, 1668583992424L },
                    { 3, "personal-2-3", 1668583992424L, 0L, false, 1668583992424L, 2, (sbyte)1, 1668583992424L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_Id",
                table: "Sessions",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Sessions",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_Id",
                table: "Sessions");

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumns: new[] { "FromId", "Id" },
                keyValues: new object[] { 1, "personal-1-2" });

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumns: new[] { "FromId", "Id" },
                keyValues: new object[] { 2, "personal-1-2" });

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumns: new[] { "FromId", "Id" },
                keyValues: new object[] { 2, "personal-2-3" });

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumns: new[] { "FromId", "Id" },
                keyValues: new object[] { 3, "personal-2-3" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sessions",
                table: "Sessions",
                columns: new[] { "FromId", "Id" });
        }
    }
}
