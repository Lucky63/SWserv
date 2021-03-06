﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapplication.Models.Data;

namespace webapplication.Models
{
	public class DBUserContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Friends> Friendships { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<FileModel> Files { get; set; }
		public DbSet<Photo> Photos { get; set; }
		public DbSet<PostAndPhoto> PostAndPhotos { get; set; }
		public DbSet<UserPost> UserPosts { get; set; }
		public DbSet<Like> Likes { get; set; }

		public DBUserContext()
		{
			Database.EnsureCreated();
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
			{
				relationship.DeleteBehavior = DeleteBehavior.Restrict;
			}

			modelBuilder.Entity<Friends>()
				.HasKey(t => new { t.UserId, t.FriendId });

			modelBuilder.Entity<Friends>()
				.HasOne(sc => sc.User)
				.WithMany(c => c.UserFriends)
				.HasForeignKey(sc => sc.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Friends>()
				.HasOne(sc => sc.Friend)
				.WithMany(s => s.WhoAddMe)
				.HasForeignKey(sc => sc.FriendId);


			modelBuilder.Entity<Like>()
				.HasKey(t => new { t.UserId, t.PostAndPhotoId });

			modelBuilder.Entity<Like>()
				.HasOne(sc => sc.PostAndPhoto)
				.WithMany(c => c.Likes)
				.HasForeignKey(sc => sc.PostAndPhotoId);

			modelBuilder.Entity<Like>()
				.HasOne(sc => sc.User)
				.WithMany(s => s.Likes)
				.HasForeignKey(sc => sc.UserId)
				.OnDelete(DeleteBehavior.Restrict);


		}



		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=SNTestDB;Trusted_Connection=True;");
		}
	}
}
