using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using webapplication.Models;

namespace webapplication.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FileController : ControllerBase
	{
		DBUserContext db;
		IHostingEnvironment _appEnvironment;

		public FileController(DBUserContext context, IHostingEnvironment appEnvironment)
		{
			db = context;
			_appEnvironment = appEnvironment;
		}

		[HttpPost("[action]/{formData}"), Route("addfile")]
		public async Task AddFile(IFormFile formData)
		{
			
				// путь к папке Files
				string path = "/Files/" + formData.FileName;
				// сохраняем файл в папку Files в каталоге wwwroot
				using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
				{
					await formData.CopyToAsync(fileStream);
				}
				FileModel file = new FileModel { Name = formData.FileName, Path = path };
				db.Files.Add(file);
				db.SaveChanges();
			
		}
	}
}
