using Microsoft.EntityFrameworkCore.Migrations;

namespace EFSQLiteDemo.Migrations
{
    public partial class AddedNewKite2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Kites",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "KiteHeight",
                table: "Kites",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "KiteWeight",
                table: "Kites",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KiteWidth",
                table: "Kites",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Kites");

            migrationBuilder.DropColumn(
                name: "KiteHeight",
                table: "Kites");

            migrationBuilder.DropColumn(
                name: "KiteWeight",
                table: "Kites");

            migrationBuilder.DropColumn(
                name: "KiteWidth",
                table: "Kites");
        }
    }
}
