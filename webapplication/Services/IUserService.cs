using System;
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
		Task DeleteFriendAsync(int userId, int friendId);
		Task<User> GetUserForAsync(int id);
		Task <User>EditAsync(User user);
		Task EditSaveAsync(User userdb);
		Task SaveUserPostAsync(string name, PostModel postText);
		Task<PostsViewModel> GetAllPostsAsync(int id, int page, int size);
		Task LikeAsync(int id, int likeid);
		Task<GetPhotosViewModel> GetUserPhotosAsync(int id, int page = 1, int size = 5);
		Task<GetUserFriendsViewModel> GetFriendsAsync(int id, int page, int size);


	}
}
