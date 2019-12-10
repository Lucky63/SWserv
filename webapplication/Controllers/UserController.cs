using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using webapplication.Models;
using webapplication.Models.Data;
using webapplication.Services;

namespace webapplication.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		DBUserContext db;
		IUserService _userService;
		IFriendService _friendService;
		IHttpContextAccessor _httpContextAccessor;
		public UserController(DBUserContext context, IUserService userService, IFriendService friendService, IHttpContextAccessor httpContextAccessor)
		{
			db = context;
			_userService = userService;
			_friendService = friendService;
			_httpContextAccessor = httpContextAccessor;
			if (!db.Users.Any())
			{
				db.Users.Add(new User { UserName = "Allan1", Password = "123"});
				db.Users.Add(new User { UserName = "Allan2", Password = "123" });
				db.Users.Add(new User { UserName = "Allan3", Password = "123" });
				db.SaveChanges();
			}
		}		

		[HttpGet("[action]/{id}")]
		public async Task<IActionResult>GetUserForMessage(int id)
		{
			var user = await _userService.GetUserForMessageAsync(id);
			return Ok(user);
		}

		[HttpPut, Route("edit")]
		public async Task <IActionResult>Edit([FromBody]User user)
		{
			User userdb = await _userService.EditAsync(user);
			userdb.UserName = user.UserName;
			userdb.LastName = user.LastName;
			userdb.Age = user.Age;
			userdb.City = user.City;
			userdb.AvatarImgPath = user.AvatarImgPath;

			if (user != null)
			{
				await _userService.EditSaveAsync(userdb);
				var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
				var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, user.UserName),
					new Claim(ClaimTypes.Role, "Manager")
				};

				var tokeOptions = new JwtSecurityToken(
					issuer: "http://localhost:5000",
					audience: "http://localhost:5000",
					claims: claims,
					expires: DateTime.Now.AddMinutes(100),
					signingCredentials: signinCredentials
				);

				var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
				return Ok(new { Token = tokenString });
			}
			else
			{
				return Unauthorized();
			}
		}

		
		//Получаю ИД друга
		[HttpGet("[action]/{id}")]
		public async Task AddFriend(int id)		
		{
			string UserIdentityName = User.Identity.Name;
			await _friendService.AddFriendAsync(id, UserIdentityName);
		}		

		[HttpDelete("[action]/{id}")]
		public async Task DeleteFriend(int id)
		{
			string IdentityUserName = User.Identity.Name;
			await _userService.DeleteFriendAsync(IdentityUserName, id);
		}


	[HttpGet("[action]"), Authorize(Roles = "Manager")]
		public async Task<IActionResult> GetIdentity()
		{
			string name = User.Identity.Name;
			var user = await _userService.GetIdentityAsync(name);
			
			var userdb = new UserViewModel()
			{
				Id = user.Id,
				UserName = user.UserName,
				LastName = user.LastName,
				Password = user.Password,
				Age = user.Age,
				City = user.City,
				AvatarImgPath =user.AvatarImgPath,				
				Friends = user.UserFriends.Select(x => new UserFriendsViewModel(x)).ToList()				
			};
			return Ok (userdb);
		}

		[HttpGet("[action]")]
		public async Task<List<User>> GetAll()
		{
			return await _userService.GetAllAsync();
		}

		[HttpGet("[action]/{id}")]
		public async Task<IActionResult> GetUserForProfile(int id)
		{
			var user = await _userService.GetUserForProfileAsync(id);

			var userdb = new UserViewModel()
			{
				Id = user.Id,
				UserName = user.UserName,
				LastName = user.LastName,
				Password = user.Password,
				Age = user.Age,
				City = user.City,
				AvatarImgPath = user.AvatarImgPath,
				Photos = user.Photos.Select(x => new PhotosViewModel(x)).ToList(),
				Friends = user.UserFriends.Select(x => new UserFriendsViewModel(x)).ToList(),
				UserPosts = user.UserPosts.Select(x => new UserPostViewModel(x)).ToList()
			};
			return Ok(userdb);
		}

		
		[HttpPost("[action]")]
		public async Task SaveUserPost([FromBody] PostModel postText)
		{
			var currentUser = await db.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
			if (currentUser != null)
			{				
				currentUser.UserPosts
					.Add(new UserPost { AuthorPost=currentUser.UserName, Post = postText.Text, TimeOfPublication = DateTime.Now, User=currentUser });
				db.Update(currentUser);
				await db.SaveChangesAsync();
			}
		}

		
		[HttpGet("[action]/{page}")]
		[HttpGet("[action]/{page}/{size}")]
		public async Task <PostsViewModel> GetAllPosts(int page, int size)		
		{		
			int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			
			if (currentUserId != 0)
			{
				var posts = await db.UserPosts
				.Where(p => p.User.UserFriends.Any(f => f.Friend.Id == currentUserId))
				.OrderByDescending(s => s.TimeOfPublication)
				.Skip((page - 1) * size)
				.Take(size)
				.Select(x => new UserPostViewModel(x)).ToListAsync();

				var count = db.UserPosts
					.Where(p => p.User.UserFriends.Any(f => f.Friend.Id == currentUserId)).Count();

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

		[HttpPost("[action]")]
		public async Task Like([FromBody]int likeid)
		{			
			int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);			

			if (currentUserId != 0 && likeid != 0)
			{
				var likeCounter = db.Likes.Where(x => x.PostAndPhotoId == likeid).Count();
				var postForLike = await db.PostAndPhotos.FirstOrDefaultAsync(x => x.Id == likeid);

				var likeForData = await db.Likes
					.Where(x => x.UserId == currentUserId && x.PostAndPhotoId == likeid)
					.Select(x => x.PostAndPhotoId).FirstOrDefaultAsync();

				if (likeForData == 0)
				{
					await db.Likes.AddAsync(new Like { PostAndPhotoId = likeid, UserId = currentUserId });
					postForLike.LikeCounter = likeCounter + 1;
				}
				else
				{
					db.Likes.Remove(new Like { UserId = currentUserId, PostAndPhotoId = likeid });
					postForLike.LikeCounter = likeCounter - 1;					
				}
				db.Update(postForLike);
				await db.SaveChangesAsync();
			}
			
		}

		[HttpGet("[action]/{id}/{page}/{size}"), Authorize(Roles = "Manager")]
		public async Task<GetPhotosViewModel> GetUserPhotos(int id, int page=1, int size=5)
		{
			int currentUserId = await db.Users.Where(x=>x.Id==id).Select(x=>x.Id).FirstOrDefaultAsync();
			var photos = await db.Photos.Where(x => x.UserId == currentUserId).Skip((page - 1) * size)
				.Take(size).ToListAsync();

			var count = db.Photos
					.Where(p => p.UserId==currentUserId).Count();

			var album = new GetPhotosViewModel
			{
				photos = photos,
				Count=count
			};

			return album;
		}

		[HttpGet("[action]"), Authorize(Roles = "Manager")]
		public async Task<int> GetIdentityUserId()
		{
			return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
		}
	}	
}
