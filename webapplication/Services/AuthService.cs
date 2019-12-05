using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapplication.Models;

namespace webapplication.Services
{
	public class AuthService : IAuthService
	{
		DBUserContext _db;
		public AuthService(DBUserContext context)
		{
			_db = context;
		}

		public async Task<User> LoginAsync(string userName)
		{
			return await _db.Users.FirstOrDefaultAsync(x => x.UserName == userName);
		}

		public async Task<User> RegistrationAsync(string userName)
		{
			return await _db.Users.FirstOrDefaultAsync(x => x.UserName == userName);
		}

		public async Task RegistrationAsyncSave(string userName, string password)
		{
			const string anonim = @"Resources\Images\AnonimPage.jpg";
			_db.Users.Add(new User { UserName = userName, Password = password, AvatarImgPath= anonim });
			await _db.SaveChangesAsync();
		}
	}
}
