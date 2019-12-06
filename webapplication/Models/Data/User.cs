using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapplication.Models.Data;

namespace webapplication.Models
{
	public class User
	{
		public User()
		{
			UserFriends = new List<Friends>();
			UserPosts = new List<UserPost>();
			LikePhotos = new List<LikePhoto>();
		}
		public int Id { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string LastName { get; set; }
		public int Age { get; set; }
		public string City { get; set; }





		public string AvatarImgPath { get; set; }
		public List<UserPost> UserPosts { get; set; }
		public List<Photo> Photos { get; set; }
		public List<Friends> UserFriends { get; set; }
		public List<Friends> WhoAddMe { get; set; }
		public List<LikePhoto> LikePhotos { get; set; }
	}
}
