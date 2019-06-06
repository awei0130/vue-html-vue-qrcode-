using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreMiddleware.Middleware
{
    public class EditMiddleware
    {
        private RequestDelegate nextDelegate;
        public EditMiddleware(RequestDelegate _nextDelegate)
        {
            nextDelegate = _nextDelegate;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Items["flag"] = httpContext.Request.Path.ToString() == "/middleware" ? true : false;
            await nextDelegate.Invoke(httpContext);
        }
    }
}
