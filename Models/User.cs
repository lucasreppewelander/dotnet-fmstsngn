using System;

namespace dotnet_fmstsngn.Models
{
	public class User : BaseEntity
	{
		public long id { get; set; }
		public string username { get; set; }
		public string? password { get; set; }
		public string token { get; set; }
	}
}