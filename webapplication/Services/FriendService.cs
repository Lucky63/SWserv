using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapplication.Models;

namespace webapplication.Services
{
	public class FriendService: IFriendService
	{
		DBUserContext _db;

		public FriendService(DBUserContext context)
		{
			_db = context;
		}

		public async Task<User> AddFriendAsync(int id)
		{
			return await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
		}
		
		public async Task<User> AddUserToFriendAsync(string UserIdentityName, User Friend)
		{
			User currentUser = await _db.Users.FirstOrDefaultAsync(x => x.UserName == UserIdentityName);
			Friend.UserFriends.Add(new Friends { UserId = Friend.Id, FriendId = currentUser.Id });
			_db.Update(currentUser);
			_db.SaveChanges();
			return Friend;
		}

		public async Task <User> AddFriendToUserAsync(string UserIdentityName, User Friend)
		{
			User currentUser = await _db.Users.FirstOrDefaultAsync(x => x.UserName == UserIdentityName);
			currentUser.UserFriends.Add(new Friends { UserId = currentUser.Id, FriendId = Friend.Id });
			_db.Update(Friend);
			_db.SaveChanges();
			return currentUser;
		}
	}
}
