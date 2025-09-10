using Microsoft.AspNetCore.Http;

namespace MrX.Web.Middleware;

public class LogRequestHeaders(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Items["Log"] is SetupLogMiddleware.Log l)
        {
            l.RequestHeaders = context.Request.Headers;
            await _next.Invoke(context);
        }
        else
        {
            throw new Exception("SetupLogMiddleware Not Found");
        }
    }
}