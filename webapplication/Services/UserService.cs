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

		public async Task<GetUserFriendsViewModel> GetAllAsync(int page, int size)///////////////////////FINISH
		{
			var allUsers = await db.Users
				.Skip((page - 1) * size)
				.Take(size)
				.ToListAsync();
			var count = db.Users.Count();
			var getUsers = new GetUserFriendsViewModel
			{
				friends = allUsers,
				Count=count
			};
			return getUsers;
		}

		public async Task<User> GetIdentityAsync(string name)///////////////////////FINISH
		{
			return await  db.Users.FirstOrDefaultAsync(x => x.UserName == name);			
		}

		public async Task<User> GetUserForAsync(int id)///////////////////////FINISH
		{
			return await db.Users.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task SaveUserPostAsync(string name, PostModel postText)
		{
			var currentUser = await db.Users.FirstOrDefaultAsync(x => x.UserName == name);
			if (currentUser != null)
			{
				currentUser.UserPosts
					.Add(new UserPost { AuthorPost = currentUser.UserName, Post = postText.Text, TimeOfPublication = DateTime.Now, User = currentUser });
				db.Update(currentUser);
				await db.SaveChangesAsync();
			}
		}
	}
}
