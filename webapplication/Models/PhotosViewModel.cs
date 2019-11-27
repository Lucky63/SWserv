using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Models
{
	public class PhotosViewModel
	{
		public PhotosViewModel(Photos photos)
		{
			id = photos.Id;
			path = photos.PhotoPath;
		}
		public int id { get; set; }
		public string path { get; set; }
	}
}
