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

namespace dotnet_fmstsngn.Filters
{
	public class AuthorizeBearerAttribute : ActionFilterAttribute
	{
		public AuthorizeBearerAttribute() { }

		public override void OnResultExecuting(ResultExecutingContext context)
		{
			bool isAuthenticated = false;
			foreach (KeyValuePair<string, StringValues> entry in context.HttpContext.Request.Headers)
			{
				if (entry.Key == "Authorization" && entry.Value == "Bearer 2uf2m8CyVZ5sxeVv5E7ALVlyv5hCDWga")
				{
					isAuthenticated = true;
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

			base.OnResultExecuting(context);
		}
	}
}