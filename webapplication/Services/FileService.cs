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

namespace webapplication.Services
{
	public class FileService : IFileService
	{

		DBUserContext _db;
		IUserService _userService;

		public FileService(DBUserContext context, IUserService userService)
		{
			_db = context;
			_userService = userService;
		}

		public async Task DeletePhotoAsync(int id)
		{
			var photo = await _db.Photos.FirstOrDefaultAsync(x => x.Id == id);
			string path = photo.PhotoPath;
			System.IO.File.Delete(path);
			_db.Remove(photo);
			await _db.SaveChangesAsync();
		}

		public async Task SavePhotoAsync(string dbPath, int id)
		{
			var user = await _userService.GetById(id);
			_db.Photos.Add(new Photo { PhotoPath = dbPath, UserId = id, AuthorPost = user.UserName, TimeOfPublication = DateTime.Now });
			await _db.SaveChangesAsync();
		}

		public async Task<string> Upload(IFormFile file)
		{

			var folderName = Path.Combine("Resources", "Images");
			var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);


			var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
			var fullPath = Path.Combine(pathToSave, fileName);
			var dbPath = Path.Combine(folderName, fileName);

			using (var stream = new FileStream(fullPath, FileMode.Create))
			{
				file.CopyTo(stream);
			}
			return dbPath;
		}

		public async Task<string> UploadPhotoAsync(IFormFile file)
		{
			var folderName = Path.Combine("Resources", "Photos");
			var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);


			var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
			var fullPath = Path.Combine(pathToSave, fileName);
			var dbPath = Path.Combine(folderName, fileName);

			using (var stream = new FileStream(fullPath, FileMode.Create))
			{
				file.CopyTo(stream);
			}
			return dbPath;
		}
	}
}
