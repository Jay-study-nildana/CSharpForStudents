using Microsoft.EntityFrameworkCore.Migrations;

namespace crudaspnetcore3point1mysql.Migrations
{
    public partial class WatchList2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserEmailAddress",
                table: "WatchList",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1) CHARACTER SET utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserEmailAddress",
                table: "WatchList",
                type: "varchar(1) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
