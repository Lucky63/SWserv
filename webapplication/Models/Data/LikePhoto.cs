using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Models.Data
{
	public class Like
	{
		public int UserId { get; set; }
		public User User { get; set; }

		public int PostAndPhotoId { get; set; }
		public PostAndPhoto PostAndPhoto { get; set; }

	}
}
