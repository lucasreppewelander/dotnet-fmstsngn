using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Text;
using dotnet_fmstsngn.Models;
using dotnet_fmstsngn.Filters;

namespace backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly ProductContext _context;
		private static CancellationToken token = new CancellationToken();

		public ProductController(ProductContext context)
		{
			_context = context;
		}

		// GET: api/Product
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			return await _context.Products.ToListAsync();
		}

		// GET: api/Product/5

		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProduct(long id)
		{
			var product = await _context.Products.FindAsync(id);

			if (product == null)
			{
				return NotFound();
			}

			return product;
		}

		[HttpGet("{id}/html")]
		public async Task<ActionResult<string>> GetProductHtml(long id)
		{
			var product = await _context.Products.FindAsync(id);
			if (product == null)
			{
				return NotFound();
			}

			string html = $"<h1 style='color: white;background-color: red;padding:8px 10px;'>{product.name}</h1>";

			return Content(html, "text/html", Encoding.UTF8);
		}

		// PUT: api/Product/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[AuthorizeBearer]
		[HttpPut("{id}")]
		public async Task<ActionResult<Product>> PutProduct(long id, Product putProduct)
		{
			if (id != putProduct.id)
			{
				return BadRequest();
			}

			Product product = await _context.Products.FirstOrDefaultAsync(p => p.id == id);

			if (putProduct.name != null)
			{
				product.name = putProduct.name;
			}

			if (putProduct.html != null)
			{
				product.html = putProduct.html;
			}

			_context.Entry(product).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync(token);
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ProductExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return Ok(product);
		}

		// POST: api/Product
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[AuthorizeBearer]
		[HttpPost]
		public async Task<ActionResult<Product>> PostProduct(Product product)
		{
			_context.Products.Add(product);
			await _context.SaveChangesAsync(token);

			return CreatedAtAction("GetProduct", new { id = product.id }, product);
		}

		// DELETE: api/Product/5
		[AuthorizeBearer]
		[HttpDelete("{id}")]
		public async Task<ActionResult<Product>> DeleteProduct(long id)
		{
			var product = await _context.Products.FindAsync(id);
			if (product == null)
			{
				return NotFound();
			}

			_context.Products.Remove(product);
			await _context.SaveChangesAsync(token);

			return product;
		}

		private bool ProductExists(long id)
		{
			return _context.Products.Any(e => e.id == id);
		}
	}
}
