using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Models
{
	public class DBUserContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Friends> Friendships { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<FileModel> Files { get; set; }
		public DbSet<Photos> Photos { get; set; }
		public DbSet<Tape>	Tapes { get; set; }
		public DbSet<UserPost> UserPosts { get; set; }

		public DBUserContext()
		{
			Database.EnsureCreated();
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Friends>()
				.HasKey(t => new { t.UserId, t.FriendId });

			modelBuilder.Entity<Friends>()
				.HasOne(sc => sc.User)
				.WithMany(c => c.UserFriends)
				.HasForeignKey(sc => sc.FriendId);

			modelBuilder.Entity<Friends>()
				.HasOne(sc => sc.Friend)
				.WithMany(s => s.WhoAddMe)
				.HasForeignKey(sc => sc.FriendId);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=SocNW;Trusted_Connection=True;");
		}
	}
}
