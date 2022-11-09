using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QianShiChat.Database.Migrations.Migrations
{
    public partial class v100 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "userinfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Account = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "账号"),
                    Password = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "密码"),
                    NickName = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false, comment: "昵称"),
                    Name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false, comment: "姓名"),
                    Avatar = table.Column<string>(type: "TEXT", nullable: false, comment: "头像")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userinfo", x => x.Id);
                },
                comment: "用户表");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userinfo");
        }
    }
}
