using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Models
{
	public class GetPhotosViewModel
	{
		public List<Photo> Photos { get; set; }
		public int Count { get; set; }
	}
}
