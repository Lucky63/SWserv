using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Models
{
	public class Photos
	{
		public int Id { get; set; }
		public string PhotoPath { get; set; }

		public int UserId { get; set; }
		public User User { get; set; }
	}
}
