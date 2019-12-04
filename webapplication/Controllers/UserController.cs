﻿using Microsoft.AspNetCore.Authorization;
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

		[HttpGet("[action]/{id}"), Route("getuserformessageasync")]
		public async Task<IActionResult>GetUserForMessageAsync(int id)
		{
			var user = await _userService.GetUserForMessageAsync(id);
			return Ok(user);
		}

		[HttpPut, Route("editasync")]
		public async Task <IActionResult>EditAsync([FromBody]User user)
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
		[HttpGet("[action]/{id}"), Route("addfriendasync")]
		public async Task AddFriendAsync(int id)
		#region Добавление друзей
		{
			User Friend = await _friendService.AddFriendAsync(id);
			await AddUserToFriendAsync(Friend);			
		}
		//Добавляю друга авторизованному пользователю
		[HttpPost]
		public async Task AddUserToFriendAsync(User Friend)
		{
			string UserIdentityName = User.Identity.Name;
			User currentFriend = await _friendService.AddUserToFriendAsync(UserIdentityName, Friend);
			await AddFriendToUserAsync(currentFriend);
		}

		public async Task AddFriendToUserAsync(User Friend)
		{
			string UserIdentityName = User.Identity.Name;
			await _friendService.AddFriendToUserAsync(UserIdentityName, Friend);			
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
				Age = user.Age,
				City = user.City,
				AvatarImgPath =user.AvatarImgPath,
				Photos=user.Photos.Select(x => new PhotosViewModel(x)).ToList(),
				Friends = user.UserFriends.Select(x => new UserFriendsViewModel(x)).ToList(),
				UserPosts = user.UserPosts.Select(x => new UserPostViewModel(x)).ToList()
			};
			return Ok (userdb);
		}

		[HttpGet, Route("getallasync")]
		public async Task<List<User>> GetAllAsync()
		{
			return await _userService.GetAllAsync();
		}

		[HttpGet("[action]/{id}"), Route("GetUserForProfileAsync")]
		public async Task<IActionResult> GetUserForProfileAsync(int id)
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

		[HttpPost("[action]/{id}"), Route("SaveUserPostAsync")]
		[HttpPost("[action]/{id}/{post}"), Route("SaveUserPostAsync")]
		public async Task SaveUserPostAsync(int id, string post)
		{
			var currentUser = await db.Users.Include(x => x.UserPosts).ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
			if (currentUser != null)
			{
				DateTime now = DateTime.Now;
				var time = now.ToString("T");
				currentUser.UserPosts
					.Add(new UserPost { AuthorPost=currentUser.UserName, Post = post, TimeOfPublication = DateTime.Now, User=currentUser });
				db.Update(currentUser);
				await db.SaveChangesAsync();
			}
		}

		[HttpGet("[action]/{id}"), Route("GetAllPostsAsync")]
		public async Task <List<UserPostViewModel>> GetAllPostsAsync(int id)		
		{
			List<UserPostViewModel> posts = new List<UserPostViewModel>();			
			User currentUser = await db.Users.Include(x => x.UserFriends).ThenInclude(x=>x.Friend).Include(x => x.UserPosts).FirstOrDefaultAsync(x => x.Id == id);
			foreach(var i in currentUser.UserFriends)
			{
				var friendsPost = await db.UserPosts.Where(x => x.UserId == i.FriendId).Select(x=> new UserPostViewModel(x)).ToListAsync(); ;
				posts.AddRange(friendsPost);
			}
			
			return posts;	

		}
	}	
}
