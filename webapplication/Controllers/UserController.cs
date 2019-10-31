using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
			User userdb = db.Users.FirstOrDefault(x => x.UserName == user.UserName);
			userdb.UserName = user.UserName;
			userdb.LastName = user.LastName;
			userdb.Password = user.Password;
			//if (user == null)
			//{
			//	return BadRequest("Invalid client request");
			//}

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
					expires: DateTime.Now.AddMinutes(5),
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
	}
}
