using Microsoft.EntityFrameworkCore.Migrations;

namespace webapplication.Migrations
{
    public partial class Addlike : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LikePhoto",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    PhotoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikePhoto", x => new { x.UserId, x.PhotoId });
                    table.ForeignKey(
                        name: "FK_LikePhoto_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LikePhoto_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LikePhoto_PhotoId",
                table: "LikePhoto",
                column: "PhotoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LikePhoto");
        }
    }
}
