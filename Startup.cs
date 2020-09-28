using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.EntityFrameworkCore;
using dotnet_fmstsngn.Models;
using dotnet_fmstsngn.Filters;

namespace backend
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ProductContext>(opt => opt.UseInMemoryDatabase("ProductList"));
			services.AddDbContext<UserContext>(opt => opt.UseInMemoryDatabase("Users"));

			services.AddScoped<AuthorizeBearerAttribute>();

			services.AddControllers();
		}

		private static async Task SeedDatabase(ProductContext context)
		{
			CancellationTokenSource source = new CancellationTokenSource();
			CancellationToken token = source.Token;

			if (context.Products.Any())
			{
				return;
			}

			var product = new Product
			{
				name = "i prefer react and node.js"
			};

			context.Add(product);
			await context.SaveChangesAsync(token);
		}

		public static async Task InitializeAsync(IServiceProvider service)
		{
			using (var scope = service.CreateScope())
			{
				var scopeProvider = scope.ServiceProvider;
				var db = scopeProvider.GetService<ProductContext>();
				await SeedDatabase(db);
			}
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				InitializeAsync(app.ApplicationServices).Wait();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
