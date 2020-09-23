using Microsoft.EntityFrameworkCore;

namespace dotnet_fmstsngn.Models
{
	public class ProductContext : DbContext
	{
		public ProductContext(DbContextOptions<ProductContext> options) : base(options)
		{

		}

		public DbSet<Product> Products { get; set; }
	}
}