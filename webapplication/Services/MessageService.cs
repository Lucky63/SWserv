using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapplication.Models;

namespace webapplication.Services
{
	public class MessageService : IMessageService
	{
		DBUserContext _db;

		public MessageService(DBUserContext db)
		{
			_db = db;
		}

		public async Task<List<string>> GetMessagesAsync(int id, int FriendId)
		{
			List<string> mesages = new List<string>();
			User User = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
			string username = User.UserName;
			User Friend = await _db.Users.FirstOrDefaultAsync(x => x.Id == FriendId);
			string friendname = Friend.UserName;

			var usermessages = _db.Messages
			  .Where(x => (x.UserId == id && x.FriendId == FriendId) ||
						  (x.UserId == FriendId && x.FriendId == id)).ToList();

			foreach (var i in usermessages)
			{
				if (i.UserId == id && i.FriendId == FriendId)
				{
					mesages.Add($"{i.dateTime}:{username}- {i.SentMessage}");
				}
				else if (i.FriendId == id && i.UserId == FriendId)
				{
					mesages.Add($"{i.dateTime}:{friendname}- {i.SentMessage}");
				}
			}

			return mesages;
		}

		public async Task SeveMessageAsync(string currentUserName, int id, string message)
		{
			User currentUser = await _db.Users.FirstOrDefaultAsync(x => x.UserName == currentUserName);
			User recipient = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
			_db.Messages.Add(new Message { UserId = currentUser.Id, FriendId = recipient.Id, SentMessage = message, dateTime = DateTime.Now });
			await _db.SaveChangesAsync();
		}
	}
}
