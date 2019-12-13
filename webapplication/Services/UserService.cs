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

		public async Task DeleteFriendAsync(int currentUserId, int id)///////////////////////FINISH
		{
			if (currentUserId != 0 && id !=0)
			{
				var UserToBeDeleted = await db.Friendships.FirstOrDefaultAsync(x => x.FriendId == id);
				var delete = await db.Friendships.FirstOrDefaultAsync(x => x.FriendId == currentUserId);
				if (UserToBeDeleted != null)
				{
					db.Friendships.Remove(UserToBeDeleted);					
					db.Friendships.Remove(delete);					
				}
				await db.SaveChangesAsync();
			}
		}

		public async Task<User> EditAsync(User user)///////////////////////FINISH
		{
			return await db.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
		}

		public async Task EditSaveAsync(User userdb)///////////////////////FINISH
		{
			db.Update(userdb);
			await db.SaveChangesAsync();
		}

		public async Task<GetUserFriendsViewModel> GetAllAsync(int page, int size)///////////////////////FINISH
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

		public async Task<User> GetIdentityAsync(string name)///////////////////////FINISH
		{
			return await  db.Users.FirstOrDefaultAsync(x => x.UserName == name);			
		}

		public async Task<User> GetUserForAsync(int id)///////////////////////FINISH
		{
			return await db.Users.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task SaveUserPostAsync(string name, PostModel postText)///////////////////////FINISH
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

		public async Task<PostsViewModel> GetAllPostsAsync(int id, int page, int size)///////////////////////FINISH
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

		public async Task LikeAsync(int id, int likeid)///////////////////////FINISH
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

		public async Task<GetPhotosViewModel> GetUserPhotosAsync(int id, int page = 1, int size = 5)///////////////////////FINISH
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

		public async Task<GetUserFriendsViewModel> GetFriendsAsync(int id, int page, int size)///////////////////////FINISH
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
