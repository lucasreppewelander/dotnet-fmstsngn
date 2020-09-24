using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dotnet_fmstsngn.Models
{
	public class ProductContext : DbContext
	{
		public ProductContext(DbContextOptions<ProductContext> options) : base(options)
		{

		}

		public DbSet<Product> Products { get; set; }

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
		{
			var entries = ChangeTracker.Entries().Where(e => e.Entity is Product && (
				e.State == EntityState.Added || e.State == EntityState.Modified
			));

			foreach (var entry in entries)
			{
				((Product)entry.Entity).modified = DateTime.Now;
				if (entry.State == EntityState.Added)
				{
					((Product)entry.Entity).created = DateTime.Now;
				}
			}

			return (await base.SaveChangesAsync(true, cancellationToken));
		}
	}
}