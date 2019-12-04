using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Models
{
	public class UserPost
	{
		public int Id { get; set; }		
		public string AuthorPost { get; set; }
		public string Post { get; set; }
		public DateTime TimeOfPublication { get; set; }

		public int UserId { get; set; }
		public User User { get; set; }
	}
}
