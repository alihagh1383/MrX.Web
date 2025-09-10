using Microsoft.AspNetCore.Http;

namespace MrX.Web.Middleware;

public class LogRequestCmd(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Items["Log"] is SetupLogMiddleware.Log l)
        {
            l.RequestMethod = context.Request.Method;
            l.RequestPath = context.Request.Path;
            l.RequestProtocol = context.Request.Protocol;
            l.RequestQuery = context.Request?.QueryString.Value ?? "";
            await _next.Invoke(context);
        }
        else
        {
            throw new Exception("SetupLogMiddleware Not Found");
        }
    }
}