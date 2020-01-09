using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace webapplication.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.CreateTable(
				name: "Files",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					Name = table.Column<string>(nullable: true),
					Path = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Files", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Messages",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					UserId = table.Column<int>(nullable: false),
					FriendId = table.Column<int>(nullable: false),
					SentMessage = table.Column<string>(nullable: true),
					dateTime = table.Column<DateTime>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Messages", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					UserName = table.Column<string>(nullable: true),
					Password = table.Column<string>(nullable: true),
					LastName = table.Column<string>(nullable: true),
					Age = table.Column<int>(nullable: false),
					City = table.Column<string>(nullable: true),
					AvatarImgPath = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Friendships",
				columns: table => new
				{
					UserId = table.Column<int>(nullable: false),
					FriendId = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Friendships", x => new { x.UserId, x.FriendId });
					table.ForeignKey(
						name: "FK_Friendships_Users_FriendId",
						column: x => x.FriendId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Friendships_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "PostAndPhotos",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					AuthorPost = table.Column<string>(nullable: true),
					TimeOfPublication = table.Column<DateTime>(nullable: false),
					LikeCounter = table.Column<int>(nullable: false),
					UserId = table.Column<int>(nullable: false),
					Discriminator = table.Column<string>(nullable: false),
					PhotoPath = table.Column<string>(nullable: true),
					Post = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_PostAndPhotos", x => x.Id);
					table.ForeignKey(
						name: "FK_PostAndPhotos_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_PostAndPhotos_Users_UserId1",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Likes",
				columns: table => new
				{
					UserId = table.Column<int>(nullable: false),
					PostAndPhotoId = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Likes", x => new { x.UserId, x.PostAndPhotoId });
					table.ForeignKey(
						name: "FK_Likes_PostAndPhotos_PostAndPhotoId",
						column: x => x.PostAndPhotoId,
						principalTable: "PostAndPhotos",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Likes_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Friendships_FriendId",
				table: "Friendships",
				column: "FriendId");

			migrationBuilder.CreateIndex(
				name: "IX_Likes_PostAndPhotoId",
				table: "Likes",
				column: "PostAndPhotoId");

			migrationBuilder.CreateIndex(
				name: "IX_PostAndPhotos_UserId",
				table: "PostAndPhotos",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_PostAndPhotos_UserId1",
				table: "PostAndPhotos",
				column: "UserId");
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "PostAndPhotos");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
