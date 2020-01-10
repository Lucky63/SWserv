using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Models
{
	public class Message
	{
		public int Id { get; set; }
		public int Sender { get; set; }
		public int Recipient { get; set; }
		public string SentMessage { get; set; }
		public DateTime DateSent { get; set; }
	}
}
