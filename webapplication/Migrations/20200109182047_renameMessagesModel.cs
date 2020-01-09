using Microsoft.EntityFrameworkCore.Migrations;

namespace webapplication.Migrations
{
    public partial class renameMessagesModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Messages",
                newName: "Sender");

            migrationBuilder.RenameColumn(
                name: "FriendId",
                table: "Messages",
                newName: "Recipient");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sender",
                table: "Messages",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Recipient",
                table: "Messages",
                newName: "FriendId");
        }
    }
}
