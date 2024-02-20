using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class MigrationOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComicBooks",
                columns: table => new
                {
                    ComicBookId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ComicBookTitle = table.Column<string>(type: "TEXT", nullable: false),
                    ComicBookYearOfRelease = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComicBooks", x => x.ComicBookId);
                });

            migrationBuilder.InsertData(
                table: "ComicBooks",
                columns: new[] { "ComicBookId", "ComicBookTitle", "ComicBookYearOfRelease" },
                values: new object[,]
                {
                    { 1, "Batman", 10 },
                    { 2, "10OFF", 10 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComicBooks");
        }
    }
}
