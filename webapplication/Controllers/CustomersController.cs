using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapplication.Models;

namespace webapplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {

		DBUserContext db;
		public CustomersController(DBUserContext context)
		{
			db = context;
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

			//db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
			return Ok(userdb);			
		}

		[HttpGet, Route("getall")]
		public IActionResult GetAll()
		{
			List<User> users = db.Users.ToList();
			return Ok(users);
			
		}
	}
}