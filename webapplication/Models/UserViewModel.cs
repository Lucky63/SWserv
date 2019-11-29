using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Models
{
	public class UserViewModel
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string LastName { get; set; }
		public int Age { get; set; }
		public string City { get; set; }
		public string AvatarImgPath { get; set; }
		public List<PhotosViewModel> Photos { get; set; }
		public List<UserFriendsViewModel> Friends { get; set; }
	}
}
