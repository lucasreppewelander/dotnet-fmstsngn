using System;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using dotnet_fmstsngn.Models;
using dotnet_fmstsngn.Filters;

namespace backend.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{

		private readonly UserContext _context;
		private static CancellationToken cancelToken = new CancellationToken();

		public AuthController(UserContext context)
		{
			_context = context;
		}

		[HttpPost("login")]
		public async Task<ActionResult<User>> Login([FromBody] LoginUser user)
		{
			var _user = _context.Users.Where(u => u.username.Equals(user.username) && u.password.Equals(user.password)).FirstOrDefault();
			if (_user != null)
			{
				User returnObject = new User()
				{
					id = _user.id,
					username = _user.username,
					token = _user.token
				};

				return Ok(returnObject);
			}

			return NotFound();
		}

		[HttpPost("register")]
		public async Task<ActionResult<User>> Register([FromBody] LoginUser user)
		{
			User _user = new User()
			{
				username = user.username,
				password = user.password,
				token = Guid.NewGuid().ToString("N")
			};

			_context.Users.Add(_user);
			await _context.SaveChangesAsync(cancelToken);

			return Ok(_user);
		}
	}
}