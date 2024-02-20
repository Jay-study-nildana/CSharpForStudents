using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class MigrationTwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ComicBookISBN",
                table: "ComicBooks",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ComicBooks",
                keyColumn: "ComicBookId",
                keyValue: 1,
                column: "ComicBookISBN",
                value: "abcd");

            migrationBuilder.UpdateData(
                table: "ComicBooks",
                keyColumn: "ComicBookId",
                keyValue: 2,
                column: "ComicBookISBN",
                value: "abcd2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComicBookISBN",
                table: "ComicBooks");
        }
    }
}
