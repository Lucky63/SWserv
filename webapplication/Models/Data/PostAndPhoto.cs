using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Models.Data
{
	public class PostAndPhoto
	{
		public PostAndPhoto()
		{
			Likes = new List<Like>();
		}

		public int Id { get; set; }
		public string AuthorPost { get; set; }



		public List<Like> Likes { get; set; }
		public DateTime TimeOfPublication { get; set; }
		public int LikeCounter { get; set; }

		public int UserId { get; set; }
		public User User { get; set; }
	}
}
