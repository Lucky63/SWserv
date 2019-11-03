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

		[HttpGet, Route("getuser")]
		public IActionResult Get()
		{
			User user = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
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


		[HttpGet("[action]/{id}"), Route("getfriend")]
		public IActionResult GetFriend(int id)
		{			
			UserViewModel user = db.Users.Select(c => new UserViewModel
			{
				Id = c.Id,
				UserName = c.UserName,
				Password = c.Password,
				LastName = c.LastName				
			}).FirstOrDefault(x => x.Id == id);			
			User thisus = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
			thisus.UserFriends.Add(new Friends {UserId=thisus.Id, FriendId = user.Id });
			db.Update(thisus);
			db.SaveChanges();
			return Ok(user);
		}
	}
}
