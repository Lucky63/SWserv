using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapplication.Models;
using webapplication.Services;

namespace webapplication.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MessagesController: Controller
	{
		DBUserContext db;
		IMessageService _messageService;
		public MessagesController(DBUserContext context, IMessageService messageService)
		{
			db = context;
			_messageService = messageService;
		}

		[HttpGet("[action]/{id}"), Route("sevemessageasync")]
		[HttpGet("[action]/{id}/{message}"), Route("sevemessageasync")]
		public async Task SeveMessageAsync(int id, string message)
		{
			string currentUserName = User.Identity.Name;
			await _messageService.SeveMessageAsync(currentUserName, id, message);			
		}

		[HttpGet("[action]/{id}"), Route("getmessagesasync")]
		[HttpGet("[action]/{id}/{friendid}"), Route("getmessagesasync")]
		public async Task <List<string>> GetMessagesAsync(int id, int FriendId)
		{
			return await _messageService.GetMessagesAsync(id, FriendId);			
		}

		
		[HttpPost("[action]/{id}"), Route("SaveMessageForTapeAsync")]
		[HttpPost("[action]/{id}/{message}"), Route("SaveMessageForTapeAsync")]
		public async Task SaveMessageForTapeAsync(int id, string message)
		{
			var currentUser = await db.Users.Include(x=>x.UserFriends).ThenInclude(x=>x.Friend).FirstOrDefaultAsync(x=>x.Id == id);
			if(currentUser != null)
			{
				foreach (var i in currentUser.UserFriends)
				{
					db.Tapes.Add(new Tape { UserId = currentUser.Id, FriendId = i.FriendId, Message = message });
					await db.SaveChangesAsync();
				}
			}

		}

		[HttpGet("[action]/{id}"), Route("GetMessagesFromTapes")]		
		public async Task<List<string>> GetMessagesFromTapes(int id)
		{
			List<string> mesages = new List<string>();
			User User = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
			string username = User.UserName;			

			var usermessages = db.Tapes
			  .Where(x => ( x.FriendId == id)||
						  (x.UserId ==  id)).ToList();

			foreach (var i in usermessages)
			{
				if (i.UserId == id)
				{
					var sentUser = db.Users.FirstOrDefault(x => x.Id == id);
					mesages.Add($"{sentUser.UserName}- {i.Message}");
				}
				if (i.FriendId == id)
				{
					var sentUser = db.Users.FirstOrDefault(x => x.Id == i.UserId);
					mesages.Add($"{sentUser.UserName}- {i.Message}");
				}
				
			}

			return mesages;

		}
	}
}
