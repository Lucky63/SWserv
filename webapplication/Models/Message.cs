using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Models
{
	public class Message
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int FriendId { get; set; }
		public string SentMessage { get; set; }
	}
}
