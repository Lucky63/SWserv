using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using webapplication.Models;
using webapplication.Services;

namespace webapplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {		
		IAuthService _authService;
		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}
		// GET api/values
		[HttpPost, Route("loginasync")]
        public async Task <IActionResult> LoginAsync([FromBody]LoginModel user)
        {
			
			User userdb = await _authService.GetUserAsync(user.UserName);
			if (user == null || userdb==null)
			{
				return BadRequest("Invalid client request");
			}			

			if (user.UserName == userdb.UserName && user.Password == userdb.Password)
			{
				var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
				var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, user.UserName),
					new Claim(ClaimTypes.Role, "Manager"),
					new Claim(ClaimTypes.NameIdentifier, userdb.Id.ToString())
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

		[HttpPost, Route("registrationasync")]
		public async Task<IActionResult> RegistrationAsync([FromBody]LoginModel user)
		{
			User userdb = await _authService.GetUserAsync(user.UserName);
			if (user == null)
			{
				return BadRequest("Invalid client request");
			}

			if (userdb == null)
			{
				await _authService.RegistrationAsyncSave(user.UserName, user.Password);
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
	}
}