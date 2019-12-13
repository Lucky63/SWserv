﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapplication.Models;

namespace webapplication.Services
{
	public interface IUserService
	{
		Task<GetUserFriendsViewModel> GetAllAsync(int page, int size);
		Task<User> GetIdentityAsync(string name);
		Task DeleteFriendAsync(int currentUserId, int id);
		Task<User> GetUserForAsync(int id);
		Task <User>EditAsync(User user);
		Task EditSaveAsync(User userdb);
		Task SaveUserPostAsync(string name, PostModel postText);


	}
}
