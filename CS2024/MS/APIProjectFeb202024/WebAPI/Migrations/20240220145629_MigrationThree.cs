using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class MigrationThree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ComicBooks",
                keyColumn: "ComicBookId",
                keyValue: 1,
                columns: new[] { "ComicBookISBN", "ComicBookTitle", "ComicBookYearOfRelease" },
                values: new object[] { "1401297242", "Batman: Hush", 2019 });

            migrationBuilder.UpdateData(
                table: "ComicBooks",
                keyColumn: "ComicBookId",
                keyValue: 2,
                columns: new[] { "ComicBookISBN", "ComicBookTitle", "ComicBookYearOfRelease" },
                values: new object[] { "1401244017", "Batman: Dark Victory", 2014 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ComicBooks",
                keyColumn: "ComicBookId",
                keyValue: 1,
                columns: new[] { "ComicBookISBN", "ComicBookTitle", "ComicBookYearOfRelease" },
                values: new object[] { "abcd", "Batman", 10 });

            migrationBuilder.UpdateData(
                table: "ComicBooks",
                keyColumn: "ComicBookId",
                keyValue: 2,
                columns: new[] { "ComicBookISBN", "ComicBookTitle", "ComicBookYearOfRelease" },
                values: new object[] { "abcd2", "10OFF", 10 });
        }
    }
}
