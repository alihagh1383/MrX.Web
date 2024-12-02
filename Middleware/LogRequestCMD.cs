using Microsoft.AspNetCore.Http;

namespace MrX.Web.Middleware;

public class LogRequestCMD
{
    private readonly RequestDelegate next;

    public LogRequestCMD(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Items["Log"] is SetupLogMiddleware.Log L)
        {
            L.Request_Method = context.Request.Method;
            L.Request_Path = context.Request.Path;
            L.Request_Protocol = context.Request.Protocol;
            L.Request_Query = context.Request.QueryString.Value;
            await next.Invoke(context);
        }
        else
        {
            throw new Exception("SetupLogMiddleware Not Found");
        }
    }
}