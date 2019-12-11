using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Models
{
	public class UserFriendsViewModel
	{
		//public UserFriendsViewModel(User user)
		//{			
		//	UserName = user.UserName;
		//	Password = user.Password;
		//	LastName = user.LastName;
		//	Age = user.Age;
		//	City = user.City;
		//	AvatarImgPath = user.AvatarImgPath;

		//	Id = user.Id;
		//}
		public UserFriendsViewModel(Friends friends)
		{
			UserId = friends.UserId;
			FriendId = friends.FriendId;			
			UserName = friends.Friend.UserName;
			Password = friends.Friend.Password;
			LastName = friends.Friend.LastName;
			Age = friends.Friend.Age;
			City = friends.Friend.City;
			AvatarImgPath = friends.Friend.AvatarImgPath;
			
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
		public int Age { get; set; }
		public string City { get; set; }
		public string AvatarImgPath { get; set; }
		public List<PhotosViewModel> Photos { get; set; }
		public List<UserPostViewModel> UserPosts { get; set; }
	}
}
