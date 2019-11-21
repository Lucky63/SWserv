using Microsoft.AspNetCore.Authorization;
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
		public UserController(DBUserContext context, IUserService userService, IFriendService friendService)
		{
			db = context;
			_userService = userService;
			_friendService = friendService;
			if (!db.Users.Any())
			{
				db.Users.Add(new User { UserName = "Allan1", Password = "123"});
				db.Users.Add(new User { UserName = "Allan2", Password = "123" });
				db.Users.Add(new User { UserName = "Allan3", Password = "123" });
				db.SaveChanges();
			}
		}		

		[HttpGet("[action]/{id}"), Route("getuserformessage")]
		public IActionResult GetUserForMessage(int id)
		{
			var user = db.Users.FirstOrDefault(x => x.Id == id);
			return Ok(user);
		}

		[HttpPut, Route("edit")]
		public IActionResult Edit([FromBody]User user)
		{
			User userdb = db.Users.FirstOrDefault(x => x.Id == user.Id);
			userdb.UserName = user.UserName;
			userdb.LastName = user.LastName;						

			if (user != null)
			{
				db.Update(userdb);
				db.SaveChanges();
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

		#region Добавление друзей
		//Получаю ИД друга
		[HttpGet("[action]/{id}"), Route("addfriendasync")]
		public async Task AddFriendAsync(int id)
		{			
			User Friend = await _friendService.AddFriendAsync(id);
			AddFriend(Friend);			
		}
		//Добавляю друга авторизованному пользователю
		[HttpPost]
		public void AddFriend(User Friend)
		{			
			User currentUser = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
			Friend.UserFriends.Add(new Friends { UserId = Friend.Id, FriendId = currentUser.Id });
			db.Update(currentUser);
			db.SaveChanges();
			FriendAddUser(Friend);
		}

		public void FriendAddUser(User Friend)
		{
			User currentUser = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
			currentUser.UserFriends.Add(new Friends { UserId = currentUser.Id, FriendId = Friend.Id });			
			db.Update(Friend);
			db.SaveChanges();
		}
		#endregion

		[HttpDelete("[action]/{id}"), Route("deletefriendasync")]
		public async Task DeleteFriendAsync(int id)
		{
			string IdentityUserName = User.Identity.Name;
			await _userService.DeleteFriendAsync(IdentityUserName, id);
		}


	[HttpGet, Route("getidentityasync"), Authorize(Roles = "Manager")]
		public async Task<IActionResult> GetIdentityAsync()
		{
			string name = User.Identity.Name;
			var user = await _userService.GetIdentityAsync(name);
			
			var userdb = new UserViewModel()
			{
				Id = user.Id,
				UserName = user.UserName,
				LastName = user.LastName,
				Password = user.Password,
				Friends = user.UserFriends.Select(x => new UserFriendsViewModel(x)).ToList()
			};
			return Ok (userdb);
		}

		[HttpGet, Route("getallasync")]
		public async Task<List<User>> GetAllAsync()
		{
			return await _userService.GetAllAsync();
		}
	}	
}
