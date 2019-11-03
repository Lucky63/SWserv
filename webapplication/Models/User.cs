using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Models
{
	public class User
	{
		public User()
		{
			UserFriends = new List<Friends>();
		}
		public int Id { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string LastName { get; set; }
		public List<Friends> UserFriends { get; set; }
		public List<Friends> WhoAddMe { get; set; }
	}
}
