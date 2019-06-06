using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreMiddleware.Middleware
{
    public class ValidateBrowserMiddleware
    {
        private RequestDelegate nextDelegate;
        public ValidateBrowserMiddleware(RequestDelegate _nextDelegate)
        {
            nextDelegate = _nextDelegate;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string  user_agent = httpContext.Request.Headers["User-Agent"].ToString();
            string agent_ = user_agent;
            Console.WriteLine(agent_ + "++++++++++");
            System.Diagnostics.Debug.WriteLine(agent_+"------------");
            //if (agent_.ToLower().Contains("trident"))
            //{
            //    httpContext.Response.StatusCode = 403;
            //}
            //else
            //{
            //    await nextDelegate.Invoke(httpContext);
            //}
            var flag = httpContext.Items["flag"] as bool?;
            System.Diagnostics.Debug.WriteLine(httpContext.Items["flag"] + "------------");
            if (flag.GetValueOrDefault())
            {
                httpContext.Response.StatusCode = 403;
            }
            else
            {
                await nextDelegate.Invoke(httpContext);
            }
        }
    }
}
