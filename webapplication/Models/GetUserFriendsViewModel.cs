﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Models
{
	public class GetUserFriendsViewModel
	{
		public List<User> friends { get; set; }
		public int Count { get; set; }
	}
}
