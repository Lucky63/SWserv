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
		IUserService _userService;

		public MessageService(DBUserContext db, IUserService userService)
		{
			_db = db;
			_userService = userService;
		}

		public async Task<GetMessageViewModel> GetMessagesAsync(int senderid, int recipientId, int page, int size)
		{
			var sender = await _userService.GetById(senderid);
			
			var recipient = await _userService.GetById(recipientId);

			var usermessages = await _db.Messages
			  .Where(x => (x.UserId == senderid && x.FriendId == recipientId) ||
						  (x.UserId == recipientId && x.FriendId == senderid))
			  .OrderByDescending(x=>x.dateTime)
			  .Skip((page - 1) * size)
			  .Take(size)
			  .OrderBy(x => x.dateTime)
			  .ToListAsync();					

			var count = _db.Messages.Where(x => (x.UserId == senderid && x.FriendId == recipientId) ||
						  (x.UserId == recipientId && x.FriendId == senderid)).Count();

			var messagesList = new GetMessageViewModel
			{
				Messages = usermessages,
				Count = count,
				SenderName=sender.UserName,
				RecipientName=recipient.UserName
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
