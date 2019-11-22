using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapplication.Controllers;
using webapplication.Models;

namespace webapplication.Services
{
	public class UserService : IUserService
	{
		DBUserContext db;
		
		public UserService(DBUserContext context)
		{
			db = context;			
		}

		public async Task DeleteFriendAsync(string IdentityUserName, int id)
		{
			User currentUser = await db.Users.Include(x => x.UserFriends).ThenInclude(x => x.Friend).FirstOrDefaultAsync(x => x.UserName == IdentityUserName);
			if (currentUser.UserFriends.Count != 0)
			{
				var UserToBeDeleted = currentUser.UserFriends.First(x => x.FriendId == id);

				if (UserToBeDeleted != null)
				{
					db.Friendships.Remove(UserToBeDeleted);
					db.SaveChanges();
					var UserToBeDeleted2 = await db.Users.Include(x => x.UserFriends).ThenInclude(x => x.Friend).FirstOrDefaultAsync(x => x.Id == UserToBeDeleted.FriendId);
					var delete = UserToBeDeleted2.UserFriends.First(x => x.FriendId == currentUser.Id);
					db.Friendships.Remove(delete);
					db.SaveChanges();
				}
			}			
		}

		public async Task<List<User>> GetAllAsync()
		{
			return await db.Users.ToListAsync();
		}

		public async Task<User> GetIdentityAsync(string name)
		{
			return await  db.Users
				.Include(x => x.UserFriends)
				.ThenInclude(x => x.Friend)
				.FirstOrDefaultAsync(x => x.UserName == name);			
		}

		public async Task<User> GetUserForMessageAsync(int id)
		{
			return await db.Users.FirstOrDefaultAsync(x => x.Id == id);
		}
	}
}
