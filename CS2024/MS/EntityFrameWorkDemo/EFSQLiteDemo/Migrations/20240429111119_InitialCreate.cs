using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFSQLiteDemo.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kites",
                columns: table => new
                {
                    KiteId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KiteColor = table.Column<int>(type: "INTEGER", nullable: false),
                    KiteDesigner = table.Column<string>(type: "TEXT", nullable: true),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    KiteHeight = table.Column<int>(type: "INTEGER", nullable: true),
                    KiteWidth = table.Column<int>(type: "INTEGER", nullable: true),
                    KiteWeight = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kites", x => x.KiteId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kites");
        }
    }
}
