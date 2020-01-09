using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapplication.Models;

namespace webapplication.Services
{
	public interface IUserService
	{
		Task<GetUserFriendsViewModel> GetAllUsersAsync(int page, int size, int id);
		Task<User> GetUserByNameAsync(string name);
		Task DeleteFriendAsync(int userId, int friendId);
		Task<User> GetById(int id);
		Task UpdateUserAsync(User updatedUser);
		Task SaveUserPostAsync(string name, PostModel postText);
		Task<PostsViewModel> GetAllPostsAsync(int id, int page, int size);
		Task LikeAsync(int userId, int likeid);
		Task<GetPhotosViewModel> GetUserPhotosAsync(int userId, int page = 1, int size = 5);
		Task<GetUserFriendsViewModel> GetFriendsAsync(int id, int page, int size);
		Task RegistrationSaveAsync(string userName, string password);
		Task AddFriendAsync(int userId, int friendId);


	}
}
