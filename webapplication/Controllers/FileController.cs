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
		public IActionResult Upload()///////////////////////FINISH
		{
			var file = Request.Form.Files[0];
			if (file.Length > 0)
			{
				var dbPath = _fileService.Upload(file).Result;
				return Ok(new { dbPath });
			}
			else
			{
				return BadRequest();
			}
		}

		[HttpPost("[action]/{id}"), Route("UploadPhotoAsync")]
		public async Task UploadPhotoAsync(int id)///////////////////////FINISH
		{
			var file = Request.Form.Files[0];
			
			var dbPath = _fileService.UploadPhotoAsync(file).Result;
				
			await _fileService.SavePhotoAsync(dbPath, id);
		}

		[HttpDelete("[action]/{id}"), Route("DeletePhotoAsync")]
		public async Task DeletePhotoAsync(int id)///////////////////////FINISH
		{
			await _fileService.DeletePhotoAsync(id);
		}
	}
}
