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

		public async Task DeleteFriendAsync(int currentUserId, int id)///////////////////////FINISH
		{
			if (currentUserId != 0 && id !=0)
			{
				var UserToBeDeleted = await db.Friendships.FirstOrDefaultAsync(x => x.FriendId == id);
				var delete = await db.Friendships.FirstOrDefaultAsync(x => x.FriendId == currentUserId);
				if (UserToBeDeleted != null)
				{
					db.Friendships.Remove(UserToBeDeleted);					
					db.Friendships.Remove(delete);					
				}
				await db.SaveChangesAsync();
			}
		}

		public async Task<User> EditAsync(User user)///////////////////////FINISH
		{
			return await db.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
		}

		public async Task EditSaveAsync(User userdb)///////////////////////FINISH
		{
			db.Update(userdb);
			await db.SaveChangesAsync();
		}

		public async Task<List<User>> GetAllAsync()
		{
			return await db.Users.ToListAsync();
		}

		public async Task<User> GetIdentityAsync(string name)///////////////////////FINISH
		{
			return await  db.Users.FirstOrDefaultAsync(x => x.UserName == name);			
		}

		public async Task<User> GetUserForMessageAsync(int id)///////////////////////FINISH
		{
			return await db.Users.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<User> GetUserForProfileAsync(int id)
		{
			return await db.Users
				.Include(x => x.UserFriends)
				.ThenInclude(x => x.Friend)
				.Include(x => x.Photos)
				.Include(x => x.UserPosts)
				.FirstOrDefaultAsync(x => x.Id == id);
		}
	}
}
