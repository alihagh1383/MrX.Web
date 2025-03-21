using Microsoft.AspNetCore.Http;

namespace MrX.Web.Middleware;

public class LogRequestBody(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Items["Log"] is SetupLogMiddleware.Log log)
        {
            context.Request.Body.Position = 0;
            log.RequestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;
            await next.Invoke(context);
        }
        else
        {
            throw new Exception("SetupLogMiddleware Not Found");
        }
    }
}