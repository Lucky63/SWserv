using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapplication.Models;

namespace webapplication.Services
{
	public interface IMessageService
	{
		Task SeveMessageAsync(int senderid, int recipientId, string message);
		Task<GetMessageViewModel> GetMessagesAsync(int senderid, int recipientId, int page, int size);
	}
}
