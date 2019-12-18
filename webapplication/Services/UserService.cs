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
					&& x.UserId== userId);

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

		public async Task<User> EditAsync(User user)
		{
			return await db.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
		}

		public async Task EditSaveAsync(User updatedUser)
		{
			db.Update(updatedUser);
			await db.SaveChangesAsync();
		}

		public async Task<GetUserFriendsViewModel> GetAllAsync(int page, int size)
		{
			var allUsers = await db.Users
				.Skip((page - 1) * size)
				.Take(size)
				.ToListAsync();
			var count = db.Users.Count();
			var getUsers = new GetUserFriendsViewModel
			{
				friends = allUsers,
				Count=count
			};
			return getUsers;
		}		

		public async Task<User> GetIdentityAsync(string name)
		{
			return await  db.Users.FirstOrDefaultAsync(x => x.UserName == name);			
		}

		public async Task<User> GetUserForAsync(int id)
		{
			return await db.Users.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task SaveUserPostAsync(string name, PostModel postText)
		{
			var currentUser = await db.Users.FirstOrDefaultAsync(x => x.UserName == name);
			if (currentUser != null)
			{
				currentUser.UserPosts
					.Add(new UserPost { AuthorPost = currentUser.UserName, Post = postText.Text, TimeOfPublication = DateTime.Now, User = currentUser });
				db.Update(currentUser);
				await db.SaveChangesAsync();
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

		public async Task LikeAsync(int id, int likeid)
		{
			if (id != 0 && likeid != 0)
			{
				var likeCounter = db.Likes.Where(x => x.PostAndPhotoId == likeid).Count();
				var postForLike = await db.PostAndPhotos.FirstOrDefaultAsync(x => x.Id == likeid);

				var likeForData = await db.Likes
					.Where(x => x.UserId == id && x.PostAndPhotoId == likeid)
					.Select(x => x.PostAndPhotoId).FirstOrDefaultAsync();

				if (likeForData == 0)
				{
					await db.Likes.AddAsync(new Like { PostAndPhotoId = likeid, UserId = id });
					postForLike.LikeCounter = likeCounter + 1;
				}
				else
				{
					db.Likes.Remove(new Like { UserId = id, PostAndPhotoId = likeid });
					postForLike.LikeCounter = likeCounter - 1;
				}
				db.Update(postForLike);
				await db.SaveChangesAsync();
			}
		}

		public async Task<GetPhotosViewModel> GetUserPhotosAsync(int id, int page = 1, int size = 5)
		{			
			var photos = await db.Photos.Where(x => x.UserId == id).Skip((page - 1) * size)
				.Take(size).ToListAsync();

			var count = db.Photos
					.Where(p => p.UserId == id).Count();

			var album = new GetPhotosViewModel
			{
				photos = photos,
				Count = count
			};

			return album;
		}

		public async Task<GetUserFriendsViewModel> GetFriendsAsync(int id, int page, int size)
		{
			var friendsList = await db.Users.Where(x => x.UserFriends.Any(z => z.FriendId == id)).Skip((page - 1) * size)
				.Take(size).ToListAsync();

			var count = db.Friendships.Where(x => x.UserId == id).Select(x => x.FriendId)
				.Count();

			var list = new GetUserFriendsViewModel
			{
				friends = friendsList,
				Count = count
			};

			return list;
		}
	}
}
