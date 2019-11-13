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
		public async Task SendToAll(string name, string message, string to)
		{
			//Clients.All.SendAsync("sendToAll", name, message);

			var userName = Context.User.Identity.Name;

			if (Context.UserIdentifier != to) // если получатель и текущий пользователь не совпадают
				await Clients.User(Context.UserIdentifier).SendAsync("Receive", name, message, userName);
			await Clients.User(to).SendAsync("Receive", name, message, userName);
		}

		public override async Task OnConnectedAsync()
		{
			await Clients.All.SendAsync("Notify", $"Приветствуем {Context.UserIdentifier}");
			await base.OnConnectedAsync();
		}
	}
}
