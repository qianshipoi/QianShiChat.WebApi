using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QianShiChat.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class v002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MessageAttachment",
                columns: table => new
                {
                    MessageId = table.Column<long>(type: "bigint", nullable: false),
                    AttachmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageAttachment", x => new { x.AttachmentId, x.MessageId });
                    table.ForeignKey(
                        name: "FK_MessageAttachment_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MessageAttachment_ChatMessage_MessageId",
                        column: x => x.MessageId,
                        principalTable: "ChatMessage",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MessageAttachment_MessageId",
                table: "MessageAttachment",
                column: "MessageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageAttachment");
        }
    }
}
