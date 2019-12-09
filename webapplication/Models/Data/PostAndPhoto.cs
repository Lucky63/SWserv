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
			LikePhotos = new List<LikePhoto>();
		}

		public int Id { get; set; }


		public int UserId { get; set; }
		public User User { get; set; }

		public List<LikePhoto> LikePhotos { get; set; }
	}
}
