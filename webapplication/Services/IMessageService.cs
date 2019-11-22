using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Services
{
	public interface IMessageService
	{
		Task SeveMessageAsync(string currentUserName, int id, string message);
		Task<List<string>> GetMessagesAsync(int id, int FriendId);
	}
}
