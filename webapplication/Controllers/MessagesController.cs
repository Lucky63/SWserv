using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

	}
}
