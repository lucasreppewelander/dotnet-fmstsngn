using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dotnet_fmstsngn.Models
{
	public class UserContext : DbContext
	{
		public UserContext(DbContextOptions<UserContext> options) : base(options)
		{

		}

		public DbSet<User> Users { get; set; }

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
		{
			var entries = ChangeTracker.Entries().Where(e => e.Entity is User && e.State == EntityState.Added);

			foreach (var entry in entries)
			{
				((User)entry.Entity).created = DateTime.Now;
			}

			return (await base.SaveChangesAsync(true, cancellationToken));
		}
	}
}