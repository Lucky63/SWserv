using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using webapplication.Models;

namespace webapplication.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MessagesController: Controller
	{
		DBUserContext db;
		public MessagesController(DBUserContext context)
		{
			db = context;
		}

		[HttpGet("[action]/{id}"), Route("sevemessage")]
		[HttpGet("[action]/{id}/{message}"), Route("sevemessage")]
		public void SeveMessage(int id, string message)
		{
			User currentUser = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
			User recipient = db.Users.FirstOrDefault(x => x.Id == id);
			db.Messages.Add(new Message { UserId = currentUser.Id, FriendId = recipient.Id, SentMessage = message, dateTime = DateTime.Now });			
			db.SaveChanges();			
		}

		[HttpGet("[action]/{id}"), Route("getmessages")]
		[HttpGet("[action]/{id}/{friendid}"), Route("getmessages")]
		public IActionResult GetMessages(int id, int FriendId)
		{
			List<string> mesages = new List<string>();
			User User = db.Users.FirstOrDefault(x => x.Id == id);
			string username = User.UserName;
			User Friend = db.Users.FirstOrDefault(x => x.Id == FriendId);
			string friendname = Friend.UserName;

			var usermessages = db.Messages
			  .Where(x => (x.UserId == id && x.FriendId == FriendId) ||
						  (x.UserId == FriendId && x.FriendId == id))
			  .ToList();

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


			return Ok(mesages);
		}

	}
}
