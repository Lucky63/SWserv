using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Models
{
	public class UserFriendsViewModel
	{
		public UserFriendsViewModel(Friends friends)
		{
			UserId = friends.UserId;
			FriendId = friends.FriendId;			
			UserName = friends.Friend.UserName;
			Password = friends.Friend.Password;
			LastName = friends.Friend.LastName;
			Id = friends.FriendId;
		}		
		public UserFriendsViewModel()
		{			
		}
		public int UserId { get; set; }
		public int FriendId { get; set; }

		public int Id { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string LastName { get; set; }
	}
}
