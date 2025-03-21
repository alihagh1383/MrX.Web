using Microsoft.AspNetCore.Http;

namespace MrX.Web.Middleware;

public class LogRequestHeaders
{
    private readonly RequestDelegate _next;

    public LogRequestHeaders(RequestDelegate next)
    {
        this._next = next;
    }

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