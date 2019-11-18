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

namespace webapplication.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		DBUserContext db;
		public UserController(DBUserContext context)
		{
			db = context;
			if (!db.Users.Any())
			{
				db.Users.Add(new User { UserName = "Allan1", Password = "123"});
				db.Users.Add(new User { UserName = "Allan2", Password = "123" });
				db.Users.Add(new User { UserName = "Allan3", Password = "123" });
				db.SaveChanges();
			}
		}

		//[HttpGet, Route("getuser")]
		//public IActionResult Get()
		//{
		//	User user = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
		//	return Ok(user);
		//}

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
		[HttpGet("[action]/{id}"), Route("getfriend")]
		public void GetFriend(int id)
		{			
			User Friend = db.Users.FirstOrDefault(x => x.Id == id);
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

		[HttpDelete("[action]/{id}"), Route("deletefriend")]
		public void Deletefriend(int id)
		{
			User currentUser = db.Users.Include(x => x.UserFriends).ThenInclude(x => x.Friend).FirstOrDefault(x => x.UserName == User.Identity.Name);
			if (currentUser.UserFriends.Count != 0)
			{
				var UserToBeDeleted = currentUser.UserFriends.First(x => x.FriendId == id);
				if (UserToBeDeleted != null)
				{
					db.Friendships.Remove(UserToBeDeleted);
					db.SaveChanges();
					var UserToBeDeleted2 = db.Users.Include(x => x.UserFriends).ThenInclude(x => x.Friend).FirstOrDefault(x => x.Id == UserToBeDeleted.FriendId);
					var delete = UserToBeDeleted2.UserFriends.First(x => x.FriendId == currentUser.Id);
					db.Friendships.Remove(delete);
					db.SaveChanges();
				}
			}
		}
	}
}
