using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MrX.Web.Middleware
{
    public class LogRequestBody
    {
        private readonly RequestDelegate next;
        public LogRequestBody(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Items["Log"] is SetupLogMiddleware.Log L)
            {
                context.Request.Body.Position = 0;
                L.Request_Body =await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0;
                await next.Invoke(context);
            }
            else
                throw new Exception("SetupLogMiddleware Not Found");

        }
    }
}
