using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapplication.Models;

namespace webapplication.Services
{
	public interface IAuthService
	{
		Task<User> LoginAsync(string userName);
		Task<User> RegistrationAsync(string userName);
		Task RegistrationAsyncSave(string userName, string password);
	}
}
