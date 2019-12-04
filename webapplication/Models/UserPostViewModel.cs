using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Models
{
	public class UserPostViewModel
	{
		public UserPostViewModel(UserPost userPost)
		{
			Id = userPost.Id;
			Post = userPost.Post;
			TimeOfPublication = userPost.TimeOfPublication;
			AuthorPost = userPost.AuthorPost;
		}
		public int Id { get; set; }
		public string AuthorPost { get; set; }
		public string Post { get; set; }
		public DateTime TimeOfPublication { get; set; }
	}
}
