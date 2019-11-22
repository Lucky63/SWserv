using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapplication.Models;

namespace webapplication.Services
{
	public interface IUserService
	{
		Task<List<User>> GetAllAsync();
		Task<User> GetIdentityAsync(string name);
		Task DeleteFriendAsync(string IdentityUserName, int id);
		Task<User> GetUserForMessageAsync(int id);
		Task <User>EditAsync(User user);
		Task EditSaveAsync(User userdb);
	}
}
