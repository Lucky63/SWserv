using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapplication.Models;

namespace webapplication.Services
{
	public interface IMessageService
	{
		Task SeveMessageAsync(int userId, int id, string message);
		Task<GetMessageViewModel> GetMessagesAsync(int id, int FriendId, int page, int size);
	}
}
