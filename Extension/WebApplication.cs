using Microsoft.AspNetCore.Builder;

namespace MrX.Web.Extension;

public static class WebApplicationExtension
{
    public static void UseMiddlewareForPath<T>(this Microsoft.AspNetCore.Builder.IApplicationBuilder app, string path) => app.UseWhen(configuration: appBuilder => appBuilder.UseMiddleware<T>(), predicate: context => context.Request.Path.StartsWithSegments(path));

}