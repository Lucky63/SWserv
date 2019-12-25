using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Models
{
	public class GetMessageViewModel
	{
		public string SenderName { get; set; }
		public string RecipientName { get; set; }
		public List<Message> Messages { get; set; }
		public int Count { get; set; }
	}
}
