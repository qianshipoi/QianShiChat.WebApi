using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QianShiChat.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class v002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<sbyte>(type: "tinyint", nullable: false),
                    FromId = table.Column<int>(type: "int", nullable: false),
                    ToId = table.Column<int>(type: "int", nullable: false),
                    MessagePosition = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<long>(type: "bigint", nullable: false),
                    UpdateTime = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeleteTime = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_UserInfo_FromId",
                        column: x => x.FromId,
                        principalTable: "UserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_FromId",
                table: "Sessions",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_IsDeleted",
                table: "Sessions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_ToId",
                table: "Sessions",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_Type",
                table: "Sessions",
                column: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sessions");
        }
    }
}
