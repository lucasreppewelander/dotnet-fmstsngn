using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Primitives;
using Microsoft.EntityFrameworkCore;
using dotnet_fmstsngn.Models;

namespace dotnet_fmstsngn.Filters
{
	public class AuthorizeBearerAttribute : ActionFilterAttribute
	{
		private readonly UserContext _context;
		public AuthorizeBearerAttribute(UserContext context)
		{
			this._context = context;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			bool isAuthenticated = false;
			foreach (KeyValuePair<string, StringValues> entry in context.HttpContext.Request.Headers)
			{
				if (entry.Key == "Authorization")
				{
					string token = entry.Value.ToString().Replace("Bearer ", "");
					var users = from u in _context.Users where u.token == token select u;

					if (users.Count() == 1)
					{
						isAuthenticated = true;
					}
				}
			}

			if (isAuthenticated == false)
			{
				context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
				context.Result = new ContentResult()
				{
					Content = "Unauthorized"
				};
			}
			else
			{
				base.OnActionExecuting(context);
			}
		}
	}
}