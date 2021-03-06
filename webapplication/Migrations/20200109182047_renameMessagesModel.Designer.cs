﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using webapplication.Models;

namespace webapplication.Migrations
{
    [DbContext(typeof(DBUserContext))]
    [Migration("20200109182047_renameMessagesModel")]
    partial class renameMessagesModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("webapplication.Models.Data.Like", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("PostAndPhotoId");

                    b.HasKey("UserId", "PostAndPhotoId");

                    b.HasIndex("PostAndPhotoId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("webapplication.Models.Data.PostAndPhoto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AuthorPost");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int>("LikeCounter");

                    b.Property<DateTime>("TimeOfPublication");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("PostAndPhotos");

                    b.HasDiscriminator<string>("Discriminator").HasValue("PostAndPhoto");
                });

            modelBuilder.Entity("webapplication.Models.FileModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<string>("Path");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("webapplication.Models.Friends", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("FriendId");

                    b.HasKey("UserId", "FriendId");

                    b.HasIndex("FriendId");

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("webapplication.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Recipient");

                    b.Property<int>("Sender");

                    b.Property<string>("SentMessage");

                    b.Property<DateTime>("dateTime");

                    b.HasKey("Id");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("webapplication.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age");

                    b.Property<string>("AvatarImgPath");

                    b.Property<string>("City");

                    b.Property<string>("LastName");

                    b.Property<string>("Password");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("webapplication.Models.Photo", b =>
                {
                    b.HasBaseType("webapplication.Models.Data.PostAndPhoto");

                    b.Property<string>("PhotoPath");

                    b.HasIndex("UserId");

                    b.HasDiscriminator().HasValue("Photo");
                });

            modelBuilder.Entity("webapplication.Models.UserPost", b =>
                {
                    b.HasBaseType("webapplication.Models.Data.PostAndPhoto");

                    b.Property<string>("Post");

                    b.HasIndex("UserId")
                        .HasName("IX_PostAndPhotos_UserId1");

                    b.HasDiscriminator().HasValue("UserPost");
                });

            modelBuilder.Entity("webapplication.Models.Data.Like", b =>
                {
                    b.HasOne("webapplication.Models.Data.PostAndPhoto", "PostAndPhoto")
                        .WithMany("Likes")
                        .HasForeignKey("PostAndPhotoId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("webapplication.Models.User", "User")
                        .WithMany("Likes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("webapplication.Models.Friends", b =>
                {
                    b.HasOne("webapplication.Models.User", "Friend")
                        .WithMany("WhoAddMe")
                        .HasForeignKey("FriendId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("webapplication.Models.User", "User")
                        .WithMany("UserFriends")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("webapplication.Models.Photo", b =>
                {
                    b.HasOne("webapplication.Models.User", "User")
                        .WithMany("Photos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("webapplication.Models.UserPost", b =>
                {
                    b.HasOne("webapplication.Models.User", "User")
                        .WithMany("UserPosts")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_PostAndPhotos_Users_UserId1")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
