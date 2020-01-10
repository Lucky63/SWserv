using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapplication.Models;
using webapplication.Services;

namespace webapplication.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MessagesController : Controller
	{
		IMessageService _messageService;
		public MessagesController(IMessageService messageService)
		{
			_messageService = messageService;
		}

		[HttpGet("[action]/{id}/{message}")]
		public async Task SeveMessage(int id, string message)
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			await _messageService.SeveMessageAsync(userId, id, message);
		}

		//[HttpGet("[action]/{id}"), Route("getmessagesasync")]
		[HttpGet("[action]/{id}/{friendid}/{page}/{size}")]
		public async Task<GetMessageViewModel> GetMessages(int id, int FriendId, int page, int size)
		{
			return await _messageService.GetMessagesAsync(id, FriendId, page, size);
		}
	}
}
