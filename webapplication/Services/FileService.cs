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

		public FileService(DBUserContext context)
		{
			_db = context;
		}

		public async Task DeletePhotoAsync(int id)///////////////////////FINISH
		{
			var currentPhoto = await _db.Photos.FirstOrDefaultAsync(x => x.Id == id);
			string pathOfFile = currentPhoto.PhotoPath;
			System.IO.File.Delete(pathOfFile);//Удаляю сам файл из папки в файловой системе
			_db.Remove(currentPhoto);
			await _db.SaveChangesAsync();
		}

		public async Task SavePhotoAsync(string dbPath, int id)///////////////////////FINISH
		{
			var currentUserName = await _db.Users.Where(x => x.Id == id).Select(x => x.UserName).FirstOrDefaultAsync();
			_db.Photos.Add(new Photo { PhotoPath = dbPath, UserId = id, AuthorPost= currentUserName, TimeOfPublication = DateTime.Now });
			await _db.SaveChangesAsync();
		}

		public async Task<string> Upload(IFormFile file)///////////////////////FINISH
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

		public async Task<string> UploadPhotoAsync(IFormFile file)///////////////////////FINISH
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
