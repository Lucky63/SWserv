using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Services
{
	public interface IFileService
	{
		Task SavePhotoAsync(string dbPath, int id);
		Task DeletePhotoAsync(int id);
		Task<string> Upload(IFormFile file);
	}
}
