using Microsoft.AspNetCore.Http;

namespace MrX.Web.Middleware
{
    public class LogRequestHeaders 
    {
        private readonly RequestDelegate next;
        public LogRequestHeaders(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Items["Log"] is SetupLogMiddleware.Log L)
            {
                L.Request_Headers = context.Request.Headers;
                await next.Invoke(context);
            }
            else
                throw new Exception("SetupLogMiddleware Not Found");

        }
    }
}
