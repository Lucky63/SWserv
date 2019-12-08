using Microsoft.EntityFrameworkCore.Migrations;

namespace webapplication.Migrations
{
    public partial class DellCity1_2_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "City2",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "City3",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City1",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City2",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City3",
                table: "Users",
                nullable: true);
        }
    }
}
