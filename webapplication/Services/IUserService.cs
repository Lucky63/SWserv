using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapplication.Models;

namespace webapplication.Services
{
	public interface IUserService
	{
		Task<List<User>> GetAll();
	}
}
