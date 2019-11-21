using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapplication.Models;

namespace webapplication.Services
{
	public interface IFriendService
	{
		Task <User> AddFriendAsync(int id);
		Task<User> AddUserToFriendAsync(string UserIdentityName, User Friend);
		Task<User> AddFriendToUserAsync(string UserIdentityName, User Friend);
	}
}
