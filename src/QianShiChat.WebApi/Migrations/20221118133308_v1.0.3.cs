using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QianShiChat.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class v103 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MessageCursor",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "sender user id"),
                    ToId = table.Column<int>(type: "int", nullable: false, comment: "receiver id (user or group)"),
                    SendType = table.Column<sbyte>(type: "tinyint", nullable: false, comment: "ToId type"),
                    LastUpdateTime = table.Column<long>(type: "bigint", nullable: false),
                    StartPosition = table.Column<long>(type: "bigint", nullable: false, comment: "message start position"),
                    CurrentPosition = table.Column<long>(type: "bigint", nullable: false, comment: "message current position")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageCursor", x => new { x.UserId, x.ToId, x.SendType });
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageCursor");
        }
    }
}
