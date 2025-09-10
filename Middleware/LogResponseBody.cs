using Microsoft.AspNetCore.Http;

namespace MrX.Web.Middleware;

public class LogResponseBody(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Items["Log"] is SetupLogMiddleware.Log l)
        {
            await _next.Invoke(context);
            context.Response.Body.Position = 0;
            l.ResponseBody = new StreamReader(context.Response.Body).ReadToEnd();
            context.Response.Body.Position = 0;
        }
        else
        {
            throw new Exception("SetupLogMiddleware Not Found");
        }
    }
}