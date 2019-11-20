using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

		public async Task<List<User>> GetAll()
		{
			return await db.Users.ToListAsync();
		}

		public List<UserViewModel> GetIdentity()
		{
			List<UserViewModel> user = db.Users
				.Include(x => x.UserFriends)
				.ThenInclude(x => x.User).ToList()
				.Select(c => new UserViewModel
			{
				Id = c.Id,
				UserName = c.UserName,
				Password = c.Password,
				LastName = c.LastName,
				Friends = c.UserFriends.Select(x => new UserFriendsViewModel(x)).ToList()
			}).ToList();
			return user;
		}
	}
}
