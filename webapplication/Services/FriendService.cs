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

		public async Task AddFriendAsync(int id, string UserIdentityName)
		{
			var Friend= await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
			var currentUser = await _db.Users.FirstOrDefaultAsync(x => x.UserName == UserIdentityName);

			var data = _db.Friendships.Where(x => 
			(x.FriendId == id && x.UserId== currentUser.Id)
			|| (x.FriendId==currentUser.Id && x.UserId==id)).Count();

			if (data == 0)
			{
				Friend.UserFriends.Add(new Friends { UserId = Friend.Id, FriendId = currentUser.Id });
				currentUser.UserFriends.Add(new Friends { UserId = currentUser.Id, FriendId = Friend.Id });
				_db.Update(currentUser);
				_db.Update(Friend);
				_db.SaveChanges();
			}

			

		}		
	}
}
