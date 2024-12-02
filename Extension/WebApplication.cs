using Microsoft.AspNetCore.Builder;

namespace MrX.Web.Extension;

public static class WebApplicationExtension
{
    public static void UseMiddlewareForPaths<T>(this IApplicationBuilder app, params string[] path)
    {
        app.UseWhen(configuration: appBuilder => appBuilder.UseMiddleware<T>(), predicate: context =>
        {
            foreach (var item in path)
                if (context.Request.Path.StartsWithSegments(item))
                    return true;
            return false;
        });
    }
}