using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapplication.Models;
using webapplication.Services;

namespace webapplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {

		DBUserContext db;
		IUserService userService;
		public CustomersController(DBUserContext context, IUserService _userService)
		{
			db = context;
			userService = _userService;
		}

		[HttpGet, Route("getidenti"), Authorize(Roles = "Manager")]
		public IActionResult Get()
		{
			List<UserViewModel> user = db.Users.Include(x => x.UserFriends).ThenInclude(x => x.User).ToList().Select(c => new UserViewModel
			{
				Id = c.Id,
				UserName = c.UserName,
				Password = c.Password,
				LastName = c.LastName,
				Friends = c.UserFriends.Select(x => new UserFriendsViewModel(x)).ToList()
			}).ToList();
			UserViewModel userdb = user.FirstOrDefault(x => x.UserName == User.Identity.Name);
			return Ok(userdb);			
		}

		[HttpGet, Route("getall")]
		public async Task<List<User>> GetAll()
		{
			var users = userService.GetAll();
			return await (users);
			
		}
	}
}