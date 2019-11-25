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

		[HttpPost("[action]"), Route("addfile")]
		public async Task AddFile([FromForm]IFormFile files)
		{			
				// путь к папке Files
				string path = "/Files/" + files.FileName;
				// сохраняем файл в папку Files в каталоге wwwroot
				using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
				{
					await files.CopyToAsync(fileStream);
				}
				FileModel file = new FileModel { Name = files.FileName, Path = path };
				db.Files.Add(file);
				db.SaveChanges();
			
		}
	}
}
