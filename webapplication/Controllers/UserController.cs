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
using Z.EntityFramework.Plus;

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
		public async Task<IActionResult>GetUserForMessage(int id)///////////////////////FINISH
		{
			var user = await _userService.GetUserForAsync(id);
			return Ok(user);
		}

		[HttpPut, Route("edit")]
		public async Task <IActionResult>Edit([FromBody]User user)///////////////////////FINISH
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
		public async Task AddFriend(int id)///////////////////////FINISH		
		{
			string UserIdentityName = User.Identity.Name;
			await _friendService.AddFriendAsync(id, UserIdentityName);
		}		

		[HttpDelete("[action]/{id}")]
		public async Task DeleteFriend(int id)///////////////////////FINISH
		{
			int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);			
			await _userService.DeleteFriendAsync(currentUserId, id);
		}


	[HttpGet("[action]"), Authorize(Roles = "Manager")]
		public async Task<IActionResult> GetIdentity()///////////////////////FINISH
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

		[HttpGet("[action]/{page}/{size}")]
		public async Task<GetUserFriendsViewModel> GetAll(int page, int size)///////////////////////FINISH
		{
			var allUsers=  await _userService.GetAllAsync(page, size);			
			return allUsers;
		}

		[HttpGet("[action]/{id}")]
		public async Task<IActionResult> GetUserForProfile(int id)///////////////////////FINISH
		{
			var user = await _userService.GetUserForAsync(id);

			var userdb = new UserViewModel()
			{
				Id = user.Id,
				UserName = user.UserName,
				LastName = user.LastName,
				Password = user.Password,
				Age = user.Age,
				City = user.City,
				AvatarImgPath = user.AvatarImgPath,				
			};
			return Ok(userdb);
		}

		
		[HttpPost("[action]")]
		public async Task SaveUserPost([FromBody] PostModel postText)///////////////////////FINISH
		{
			var name = User.Identity.Name;
			await _userService.SaveUserPostAsync(name, postText);
		}

		
		[HttpGet("[action]/{page}")]
		[HttpGet("[action]/{page}/{size}")]
		public async Task <PostsViewModel> GetAllPosts(int page, int size)///////////////////////FINISH
		{		
			int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			return await _userService.GetAllPostsAsync(currentUserId, page, size);			
		}

		[HttpPost("[action]")]
		public async Task Like([FromBody]int likeid)///////////////////////FINISH
		{			
			int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			await _userService.LikeAsync(currentUserId, likeid);			
		}

		[HttpGet("[action]/{id}/{page}/{size}"), Authorize(Roles = "Manager")]
		public async Task<GetPhotosViewModel> GetUserPhotos(int id, int page=1, int size=5)///////////////////////FINISH
		{
			return await _userService.GetUserPhotosAsync(id, page, size);			
		}

		[HttpGet("[action]"), Authorize(Roles = "Manager")]
		public async Task<int> GetIdentityUserId()
		{
			return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
		}

		[HttpGet("[action]/{id}/{page}/{size}"), Authorize(Roles = "Manager")]
		public async Task <GetUserFriendsViewModel> GetFriends(int id, int page, int size)
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
