using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Models
{
	public class PostsViewModel
	{
		public List<UserPostViewModel> userPostViewModels { get; set; }
		public List<int> TotalPage { get; set; }
	}
}
