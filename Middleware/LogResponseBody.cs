using Microsoft.AspNetCore.Http;

namespace MrX.Web.Middleware
{
    public class LogResponseBody
    {
        private readonly RequestDelegate next;
        public LogResponseBody(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Items["Log"] is SetupLogMiddleware.Log L)
            {      
                await next.Invoke(context);
                context.Response.Body.Position = 0;
                L.Response_Body = new StreamReader(context.Response.Body).ReadToEnd();
                context.Response.Body.Position = 0;
            }
            else
                throw new Exception("SetupLogMiddleware Not Found");

        }
    }
}
