using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreMiddleware.Middleware
{
    public class StatusCodeMiddleware
    {
        private RequestDelegate nextDelegate;

        public StatusCodeMiddleware(RequestDelegate _nextDelegate)
        {
            nextDelegate = _nextDelegate;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await nextDelegate.Invoke(httpContext);
            switch (httpContext.Response.StatusCode)
            {
                case 403:
                    httpContext.Response.StatusCode = 200;
                    await httpContext.Response.WriteAsync("statuscode:200", Encoding.UTF8);
                    break;
                case 404:
                    await httpContext.Response.WriteAsync("statuscode:404", Encoding.UTF8);
                    break;
            }

        }
    }
}
