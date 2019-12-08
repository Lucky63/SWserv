using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Models.Data
{
	public class LikePhoto
	{
		public int UserId { get; set; }
		public User User { get; set; }

		public int PhotoId { get; set; }
		public  Photo Photo { get; set; }

	}
}
