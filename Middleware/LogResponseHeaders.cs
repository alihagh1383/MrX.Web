using Microsoft.AspNetCore.Http;

namespace MrX.Web.Middleware;

public class LogResponseHeaders(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Items["Log"] is SetupLogMiddleware.Log l)
        {
            await _next.Invoke(context);

            l.ResponseHeaders = context.Response.Headers;
        }
        else
        {
            throw new Exception("SetupLogMiddleware Not Found");
        }
    }
}