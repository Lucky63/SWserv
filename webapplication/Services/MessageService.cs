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

		public async Task<GetMessageViewModel> GetMessagesAsync(int id, int friendId, int page, int size)
		{
			var messages = new List<string>();
			var username = await _db.Users
				.Where(x => x.Id == id)
				.Select(x => x.UserName)
				.FirstOrDefaultAsync();
			
			var friendname = await _db.Users
				.Where(x => x.Id == friendId)
				.Select(x=>x.UserName)
				.FirstOrDefaultAsync();			

			var usermessages = await _db.Messages
			  .Where(x => (x.UserId == id && x.FriendId == friendId) ||
						  (x.UserId == friendId && x.FriendId == id))
			  .OrderByDescending(x=>x.dateTime)
			  .Skip((page - 1) * size)
			  .Take(size)
			  .OrderBy(x => x.dateTime)
			  .ToListAsync();

			foreach (var i in usermessages)
			{
				if (i.UserId == id && i.FriendId == friendId)
				{
					messages.Add($"{i.dateTime}:{username}- {i.SentMessage}");
				}
				else if (i.FriendId == id && i.UserId == friendId)
				{
					messages.Add($"{i.dateTime}:{friendname}- {i.SentMessage}");
				}
			}

			var count = _db.Messages.Where(x => (x.UserId == id && x.FriendId == friendId) ||
						  (x.UserId == friendId && x.FriendId == id)).Count();

			var messagesList = new GetMessageViewModel
			{
				Messages = messages,
				Count = count
			};

			return messagesList;
		}

		public async Task SeveMessageAsync(int userId, int id, string message)
		{
			_db.Messages.Add(new Message { UserId = userId, FriendId = id, SentMessage = message, dateTime = DateTime.Now });
			await _db.SaveChangesAsync();
		}
	}
}
