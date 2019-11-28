using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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

		[HttpPost("[action]"), Route("Upload")]
		public IActionResult Upload()
		{
			try
			{
				var file = Request.Form.Files[0];
				var folderName = Path.Combine("Resources", "Images");
				var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

				if (file.Length > 0)
				{
					var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
					var fullPath = Path.Combine(pathToSave, fileName);
					var dbPath = Path.Combine(folderName, fileName);

					using (var stream = new FileStream(fullPath, FileMode.Create))
					{
						file.CopyTo(stream);
					}

					return Ok(new { dbPath });
				}
				else
				{
					return BadRequest();
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpPost("[action]/{id}"), Route("UploadPhoto")]
		public async Task UploadPhoto(int id)
		{			
				var file = Request.Form.Files[0];
				var folderName = Path.Combine("Resources", "Photos");
				var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

				
					var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
					var fullPath = Path.Combine(pathToSave, fileName);
					var dbPath = Path.Combine(folderName, fileName);
					
					using (var stream = new FileStream(fullPath, FileMode.Create))
					{
						file.CopyTo(stream);
					}
			db.Photos.Add(new Photos { PhotoPath = dbPath, UserId = id });
			await db.SaveChangesAsync();
		}

		[HttpDelete("[action]/{id}"), Route("DeletePhotoAsync")]
		public async Task DeletePhotoAsync(int id)
		{
			var currentPhoto = await db.Photos.FirstOrDefaultAsync(x => x.Id == id);
			string pathOfFile = currentPhoto.PhotoPath;
			System.IO.File.Delete(pathOfFile);//Удаляю сам файл из папки в файловой системе
			db.Remove(currentPhoto);
			await db.SaveChangesAsync();
		}
	}
}
