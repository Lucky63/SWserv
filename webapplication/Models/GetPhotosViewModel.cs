using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Models
{
	public class GetPhotosViewModel
	{
		public List<Photo> photos { get; set; }
		public int Count { get; set; }
	}
}
