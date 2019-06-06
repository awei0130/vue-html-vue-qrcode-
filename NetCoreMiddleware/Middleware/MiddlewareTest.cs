using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreMiddleware.Middleware
{
    public class MiddlewareTest
    {
        private RequestDelegate nextDelegate;
        public MiddlewareTest(RequestDelegate _nextDelegate)
        {
            nextDelegate = _nextDelegate;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if(httpContext.Request.Path.ToString().ToLower() == "/middleware")
            {
                await httpContext.Response.WriteAsync(
                    "MiddlewareTest", Encoding.UTF8);
            }
            else
            {
                await nextDelegate.Invoke(httpContext);
            }
        }
    }
}
