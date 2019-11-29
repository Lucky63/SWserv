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
using webapplication.Services;

namespace webapplication.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FileController : ControllerBase
	{			
		IFileService _fileService;

		public FileController(IFileService fileService)
		{			
			_fileService = fileService;
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

		[HttpPost("[action]/{id}"), Route("UploadPhotoAsync")]
		public async Task UploadPhotoAsync(int id)
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
			await _fileService.SavePhotoAsync(dbPath, id);
		}

		[HttpDelete("[action]/{id}"), Route("DeletePhotoAsync")]
		public async Task DeletePhotoAsync(int id)
		{
			await _fileService.DeletePhotoAsync(id);
		}
	}
}
