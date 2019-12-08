using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Models
{
	public class PhotosViewModel
	{
		public PhotosViewModel(Photo photos)
		{
			id = photos.Id;
			path = photos.PhotoPath;
			likeCounter = photos.LikeCounter;
		}
		public int id { get; set; }
		public string path { get; set; }
		public int likeCounter { get; set; }
	}
}
