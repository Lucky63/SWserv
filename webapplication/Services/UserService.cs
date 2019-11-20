using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapplication.Models;

namespace webapplication.Services
{
	public class UserService : IUserService
	{
		DBUserContext db;
		public UserService(DBUserContext context)
		{
			db = context;
		}

		public async Task<List<User>> GetAll()
		{
			return await db.Users.ToListAsync();
		}
	}
}
