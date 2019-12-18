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

		public async Task AddFriendAsync(int userId, int friendId)
		{
			var friend= await _db.Users.FirstOrDefaultAsync(x => x.Id == friendId);
			var currentUser = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);

			var data = _db.Friendships.Where(x => 
			(x.FriendId == friendId && x.UserId== currentUser.Id)
			|| (x.FriendId==currentUser.Id && x.UserId== friendId)).Count();

			if (data == 0)
			{
				if (friend != null && currentUser != null)
				{
					friend.UserFriends.Add(new Friends { UserId = friendId, FriendId = userId });
					currentUser.UserFriends.Add(new Friends { UserId = userId, FriendId = friendId });					
					_db.SaveChanges();
				}
				else
				{
					//обработать ошибку отсутствующего юзера					
				}
			}
			else
			{
				//обработать ошибку если юзер уже добавлен
			}
		}		
	}
}
