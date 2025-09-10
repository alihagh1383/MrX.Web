using Microsoft.AspNetCore.Http;

namespace MrX.Web.Middleware;

public class LogRequestCookies(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Items["Log"] is SetupLogMiddleware.Log l)
        {
            l.RequestCookies = context.Request.Cookies.ToDictionary();
            await _next.Invoke(context);
        }
        else
        {
            throw new Exception("SetupLogMiddleware Not Found");
        }
    }
}