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
				db.Users.Add(new User { UserName = "Bill", Password = "123", AvatarImgPath= @"Resources\Images\AnonimPage.jpg"});
				db.Users.Add(new User { UserName = "Bob", Password = "123", AvatarImgPath = @"Resources\Images\AnonimPage.jpg"});
				db.Users.Add(new User { UserName = "Sam", Password = "123", AvatarImgPath = @"Resources\Images\AnonimPage.jpg"});
				db.SaveChanges();
			}
		}		

		[HttpGet("[action]/{id}")]
		public async Task<IActionResult>GetUserForMessage(int id)
		{
			if (id != 0)
			{
				var user = await _userService.GetUserForAsync(id);
				return Ok(user);
			}
			else
			{
				return Ok();//Обработать исключение
			}
			
		}

		[HttpPut, Route("edit")]
		public async Task <IActionResult>Edit([FromBody]User user)
		{
			if (user != null)
			{

				User updatedUser = await _userService.EditAsync(user);
				updatedUser.UserName = user.UserName;
				updatedUser.LastName = user.LastName;
				updatedUser.Age = user.Age;
				updatedUser.City = user.City;
				updatedUser.AvatarImgPath = user.AvatarImgPath;
			
				await _userService.EditSaveAsync(updatedUser);
				var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
				var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, user.UserName),
					new Claim(ClaimTypes.Role, "Manager"),
					new Claim(ClaimTypes.NameIdentifier, updatedUser.Id.ToString())
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
		[HttpGet("[action]/{friendId}")]
		public async Task AddFriend(int friendId)
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			await _friendService.AddFriendAsync(userId, friendId);
		}		

		[HttpDelete("[action]/{friendId}")]
		public async Task DeleteFriend(int friendId)
		{
			int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);			
			await _userService.DeleteFriendAsync(userId, friendId);
		}


		[HttpGet("[action]"), Authorize(Roles = "Manager")]
		public async Task<IActionResult> GetIdentityUser()
		{
			string name = User.Identity.Name;
			var user = await _userService.GetIdentityUserAsync(name);
			
			var identityUser = new UserViewModel()
			{
				Id = user.Id,
				UserName = user.UserName,
				LastName = user.LastName,
				Password = user.Password,
				Age = user.Age,
				City = user.City,
				AvatarImgPath =user.AvatarImgPath
			};
			return Ok (identityUser);
		}

		[HttpGet("[action]/{page}/{size}")]
		public async Task<GetUserFriendsViewModel> GetAll(int page, int size)
		{
			var allUsers=  await _userService.GetAllAsync(page, size);			
			return allUsers;
		}

		[HttpGet("[action]/{id}")]
		public async Task<IActionResult> GetUserForProfile(int id)
		{
			var user = await _userService.GetUserForAsync(id);

			var userForProfile = new UserViewModel()
			{
				Id = user.Id,
				UserName = user.UserName,
				LastName = user.LastName,
				Password = user.Password,
				Age = user.Age,
				City = user.City,
				AvatarImgPath = user.AvatarImgPath,				
			};
			return Ok(userForProfile);
		}

		
		[HttpPost("[action]")]
		public async Task SaveUserPost([FromBody] PostModel postText)
		{
			var name = User.Identity.Name;
			await _userService.SaveUserPostAsync(name, postText);
		}

		
		[HttpGet("[action]/{page}")]
		[HttpGet("[action]/{page}/{size}")]
		public async Task <PostsViewModel> GetAllPosts(int page, int size)
		{		
			int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			return await _userService.GetAllPostsAsync(currentUserId, page, size);			
		}

		[HttpPost("[action]")]
		public async Task Like([FromBody]int likeid)
		{			
			int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			await _userService.LikeAsync(currentUserId, likeid);			
		}

		[HttpGet("[action]/{id}/{page}/{size}"), Authorize(Roles = "Manager")]
		public async Task<GetPhotosViewModel> GetUserPhotos(int id, int page=1, int size=5)
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
			return await _userService.GetFriendsAsync(id, page, size);	
		}
	}	
}
