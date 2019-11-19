﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using webapplication.Models;

namespace webapplication.Migrations
{
    [DbContext(typeof(DBUserContext))]
    partial class DBUserContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("webapplication.Models.Friends", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("FriendId");

                    b.Property<int?>("UserId1");

                    b.HasKey("UserId", "FriendId");

                    b.HasIndex("FriendId");

                    b.HasIndex("UserId1");

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("webapplication.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FriendId");

                    b.Property<string>("SentMessage");

                    b.Property<int>("UserId");

                    b.Property<DateTime>("dateTime");

                    b.HasKey("Id");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("webapplication.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LastName");

                    b.Property<string>("Password");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("webapplication.Models.Friends", b =>
                {
                    b.HasOne("webapplication.Models.User", "Friend")
                        .WithMany("WhoAddMe")
                        .HasForeignKey("FriendId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("webapplication.Models.User", "User")
                        .WithMany("UserFriends")
                        .HasForeignKey("UserId1");
                });
#pragma warning restore 612, 618
        }
    }
}
