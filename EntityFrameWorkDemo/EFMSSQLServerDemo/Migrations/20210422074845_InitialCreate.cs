using Microsoft.EntityFrameworkCore.Migrations;

namespace EFMSSQLServerDemo.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kites",
                columns: table => new
                {
                    KiteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KiteColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KiteDesigner = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kites", x => x.KiteId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kites");
        }
    }
}
