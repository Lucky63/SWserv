using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapplication.Models;

namespace webapplication.Services
{
	public interface IFriendService
	{
		Task AddFriendAsync(int userId, int friendId);		
	}
}
