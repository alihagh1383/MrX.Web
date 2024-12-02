using Microsoft.AspNetCore.Http;

namespace MrX.Web.Middleware;

public class LogResponseHeaders
{
    private readonly RequestDelegate next;

    public LogResponseHeaders(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Items["Log"] is SetupLogMiddleware.Log L)
        {
            await next.Invoke(context);

            L.Response_Headers = context.Response.Headers;
        }
        else
        {
            throw new Exception("SetupLogMiddleware Not Found");
        }
    }
}