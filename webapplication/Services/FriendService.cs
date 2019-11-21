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
	}
}
