using System;

namespace dotnet_fmstsngn.Models
{
	public class Product : BaseEntity
	{
		public long id { get; set; }
		public string name { get; set; }
		public string html { get; set; }

		public Product()
		{

		}
	}
}