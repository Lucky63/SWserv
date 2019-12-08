using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapplication.Models.Data;

namespace webapplication.Models
{
	public class Photo
	{
		public Photo()
		{			
			LikePhotos = new List<LikePhoto>();
		}
		

		public int Id { get; set; }
		public string PhotoPath { get; set; }
		public int LikeCounter { get; set; }

		public int UserId { get; set; }
		public User User { get; set; }
		public List<LikePhoto> LikePhotos { get; set; }
	}
}
