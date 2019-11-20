using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication
{
	public class ChatHub : Hub
	{
		[Authorize]
		public async Task SendToAll(string name, string message, string to, string dateTime)
		{
			var userName = Context.User.Identity.Name;			

			if (Context.UserIdentifier != to) // если получатель и текущий пользователь не совпадают
				await Clients.User(userName).SendAsync("Receive", dateTime, name, message);

			await Clients.User(to).SendAsync("Receive", dateTime, name, message);
		}
		
	}
}
