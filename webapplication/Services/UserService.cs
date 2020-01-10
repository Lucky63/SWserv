using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapplication.Controllers;
using webapplication.Models;
using webapplication.Models.Data;

namespace webapplication.Services
{
	public class UserService : IUserService
	{
		DBUserContext db;

		public UserService(DBUserContext context)
		{
			db = context;
		}

		public async Task DeleteFriendAsync(int userId, int friendId)
		{
			if (userId != 0 && friendId != 0)
			{
				var currentUser = await db.Friendships
					.FirstOrDefaultAsync(x => x.FriendId == friendId
					&& x.UserId == userId);

				var friend = await db.Friendships
					.FirstOrDefaultAsync(x => x.FriendId == userId
					&& x.UserId == friendId);

				if (currentUser != null && friend != null)
				{
					db.Friendships.RemoveRange(currentUser, friend);
					await db.SaveChangesAsync();
				}
				else
				{
					//Обработать исключение
				}
			}
			else
			{
				//Обработать исключение
			}
		}

		public async Task<User> GetById(int id)
		{
			return await db.Users.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task UpdateUserAsync(User updatedUser)
		{
			db.Update(updatedUser);
			await db.SaveChangesAsync();
		}

		public async Task<GetUserFriendsViewModel> GetAllUsersAsync(int page, int size, int id)
		{

			var allUsers = await db.Users.Except(db.Users.Where(x => x.Id == id)).Except(db.Users
				.Where(x => x.UserFriends.Any(z => z.FriendId == id)))
				.Skip((page - 1) * size)
				.Take(size)
				.ToListAsync();

			var count = db.Users.Except(db.Users
				.Where(x => x.UserFriends.Any(z => z.FriendId == id))).Count();
			var users = new GetUserFriendsViewModel
			{
				friends = allUsers,
				Count = count
			};
			return users;
		}

		public async Task<User> GetUserByNameAsync(string name)
		{
			return await db.Users.FirstOrDefaultAsync(x => x.UserName == name);
		}

		public async Task SaveUserPostAsync(string name, PostModel postText)
		{
			var currentUser = await GetUserByNameAsync(name);
			if (currentUser != null)
			{
				currentUser.UserPosts
					.Add(new UserPost
					{
						AuthorPost = currentUser.UserName,
						Post = postText.Text,
						TimeOfPublication = DateTime.Now,
						User = currentUser
					});
				await db.SaveChangesAsync();
			}
			else
			{
				//Обработать исключение
			}
		}

		public async Task<PostsViewModel> GetAllPostsAsync(int id, int page, int size)
		{
			if (id != 0)
			{
				var posts = await db.UserPosts
				.Where(p => p.User.UserFriends.Any(f => f.Friend.Id == id))
				.OrderByDescending(s => s.TimeOfPublication)
				.Skip((page - 1) * size)
				.Take(size)
				.Select(x => new UserPostViewModel(x)).ToListAsync();

				var count = db.UserPosts
					.Where(p => p.User.UserFriends.Any(f => f.Friend.Id == id)).Count();

				var postsViewModel = new PostsViewModel
				{
					userPostViewModels = posts,
					Count = count
				};

				return postsViewModel;
			}
			else
			{
				return new PostsViewModel();
			}
		}

		public async Task LikeAsync(int userId, int likeid)
		{
			if (userId != 0 && likeid != 0)
			{
				var likeCounter = db.Likes.Where(x => x.PostAndPhotoId == likeid).Count();
				var postForLike = await db.PostAndPhotos.FirstOrDefaultAsync(x => x.Id == likeid);

				var likeForData = await db.Likes
					.Where(x => x.UserId == userId && x.PostAndPhotoId == likeid)
					.Select(x => x.PostAndPhotoId).FirstOrDefaultAsync();

				if (likeForData == 0)
				{
					await db.Likes.AddAsync(new Like { PostAndPhotoId = likeid, UserId = userId });
					postForLike.LikeCounter = likeCounter + 1;
				}
				else
				{
					db.Likes.Remove(new Like { UserId = userId, PostAndPhotoId = likeid });
					postForLike.LikeCounter = likeCounter - 1;
				}
				await db.SaveChangesAsync();
			}
			else
			{
				//Обработать исключение
			}
		}

		public async Task<GetPhotosViewModel> GetUserPhotosAsync(int userId, int page = 1, int size = 5)
		{
			var photos = await db.Photos.Where(x => x.UserId == userId).Skip((page - 1) * size)
				.Take(size).ToListAsync();

			var count = db.Photos
					.Where(p => p.UserId == userId).Count();

			var album = new GetPhotosViewModel
			{
				Photos = photos,
				Count = count
			};

			return album;
		}

		public async Task<GetUserFriendsViewModel> GetFriendsAsync(int userId, int page, int size)
		{
			var friendsList = await db.Users.Where(x => x.UserFriends.Any(z => z.FriendId == userId)).Skip((page - 1) * size)
				.Take(size).ToListAsync();

			var count = db.Friendships.Where(x => x.UserId == userId).Select(x => x.FriendId)
				.Count();

			var friends = new GetUserFriendsViewModel
			{
				friends = friendsList,
				Count = count
			};

			return friends;
		}

		public async Task RegistrationSaveAsync(string userName, string password)
		{
			const string anonim = @"Resources\Images\AnonimPage.jpg";
			db.Users.Add(new User { UserName = userName, Password = password, AvatarImgPath = anonim });
			await db.SaveChangesAsync();
		}

		public async Task AddFriendAsync(int userId, int friendId)
		{
			var friend = await GetById(friendId);
			var currentUser = await GetById(userId);

			var friendshipsCount = db.Friendships.Where(x =>
			x.FriendId == friendId && x.UserId == currentUser.Id
			|| (x.FriendId == currentUser.Id && x.UserId == friendId)).Count();

			if (friendshipsCount == 0)
			{
				if (friend != null && currentUser != null)
				{
					friend.UserFriends.Add(new Friends { UserId = friendId, FriendId = userId });
					currentUser.UserFriends.Add(new Friends { UserId = userId, FriendId = friendId });
					db.SaveChanges();
				}
				else
				{
					//обработать ошибку отсутствующего юзера					
				}
			}
			else
			{
				//обработать ошибку если юзер уже добавлен
			}
		}
	}
}
