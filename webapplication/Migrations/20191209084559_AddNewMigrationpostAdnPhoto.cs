using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace webapplication.Migrations
{
	public partial class AddNewMigrationpostAdnPhoto : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_LikePhotos_Photos_PhotoId",
				table: "LikePhotos");

			migrationBuilder.DropForeignKey(
				name: "FK_UserPosts_Users_UserId",
				table: "UserPosts");

			migrationBuilder.DropTable(
				name: "Photos");

			migrationBuilder.DropTable(
				name: "Tapes");

			migrationBuilder.DropPrimaryKey(
				name: "PK_UserPosts",
				table: "UserPosts");

			migrationBuilder.RenameTable(
				name: "UserPosts",
				newName: "PostAndPhotos");

			migrationBuilder.RenameColumn(
				name: "PhotoId",
				table: "LikePhotos",
				newName: "PostAndPhotoId");

			migrationBuilder.RenameIndex(
				name: "IX_LikePhotos_PhotoId",
				table: "LikePhotos",
				newName: "IX_LikePhotos_PostAndPhotoId");

			migrationBuilder.RenameIndex(
				name: "IX_UserPosts_UserId",
				table: "PostAndPhotos",
				newName: "IX_PostAndPhotos_UserId1");

			migrationBuilder.AlterColumn<DateTime>(
				name: "TimeOfPublication",
				table: "PostAndPhotos",
				nullable: true,
				oldClrType: typeof(DateTime));

			migrationBuilder.AddColumn<string>(
				name: "Discriminator",
				table: "PostAndPhotos",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<int>(
				name: "LikeCounter",
				table: "PostAndPhotos",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "PhotoPath",
				table: "PostAndPhotos",
				nullable: true);

			migrationBuilder.AddPrimaryKey(
				name: "PK_PostAndPhotos",
				table: "PostAndPhotos",
				column: "Id");

			migrationBuilder.CreateIndex(
				name: "IX_PostAndPhotos_UserId",
				table: "PostAndPhotos",
				column: "UserId");

			migrationBuilder.AddForeignKey(
				name: "FK_LikePhotos_PostAndPhotos_PostAndPhotoId",
				table: "LikePhotos",
				column: "PostAndPhotoId",
				principalTable: "PostAndPhotos",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_PostAndPhotos_Users_UserId",
				table: "PostAndPhotos",
				column: "UserId",
				principalTable: "Users",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_PostAndPhotos_Users_UserId1",
				table: "PostAndPhotos",
				column: "UserId",
				principalTable: "Users",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_LikePhotos_PostAndPhotos_PostAndPhotoId",
				table: "LikePhotos");

			migrationBuilder.DropForeignKey(
				name: "FK_PostAndPhotos_Users_UserId",
				table: "PostAndPhotos");

			migrationBuilder.DropForeignKey(
				name: "FK_PostAndPhotos_Users_UserId1",
				table: "PostAndPhotos");

			migrationBuilder.DropPrimaryKey(
				name: "PK_PostAndPhotos",
				table: "PostAndPhotos");

			migrationBuilder.DropIndex(
				name: "IX_PostAndPhotos_UserId",
				table: "PostAndPhotos");

			migrationBuilder.DropColumn(
				name: "Discriminator",
				table: "PostAndPhotos");

			migrationBuilder.DropColumn(
				name: "LikeCounter",
				table: "PostAndPhotos");

			migrationBuilder.DropColumn(
				name: "PhotoPath",
				table: "PostAndPhotos");

			migrationBuilder.RenameTable(
				name: "PostAndPhotos",
				newName: "UserPosts");

			migrationBuilder.RenameColumn(
				name: "PostAndPhotoId",
				table: "LikePhotos",
				newName: "PhotoId");

			migrationBuilder.RenameIndex(
				name: "IX_LikePhotos_PostAndPhotoId",
				table: "LikePhotos",
				newName: "IX_LikePhotos_PhotoId");

			migrationBuilder.RenameIndex(
				name: "IX_PostAndPhotos_UserId1",
				table: "UserPosts",
				newName: "IX_UserPosts_UserId");

			migrationBuilder.AlterColumn<DateTime>(
				name: "TimeOfPublication",
				table: "UserPosts",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldNullable: true);

			migrationBuilder.AddPrimaryKey(
				name: "PK_UserPosts",
				table: "UserPosts",
				column: "Id");

			migrationBuilder.CreateTable(
				name: "Photos",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					LikeCounter = table.Column<int>(nullable: false),
					PhotoPath = table.Column<string>(nullable: true),
					UserId = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Photos", x => x.Id);
					table.ForeignKey(
						name: "FK_Photos_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Tapes",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					FriendId = table.Column<int>(nullable: false),
					Message = table.Column<string>(nullable: true),
					UserId = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Tapes", x => x.Id);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Photos_UserId",
				table: "Photos",
				column: "UserId");

			migrationBuilder.AddForeignKey(
				name: "FK_LikePhotos_Photos_PhotoId",
				table: "LikePhotos",
				column: "PhotoId",
				principalTable: "Photos",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_UserPosts_Users_UserId",
				table: "UserPosts",
				column: "UserId",
				principalTable: "Users",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}
	}
}
