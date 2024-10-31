namespace MrX.Extensions;

public static class WebApplication
{
    public static void UseMiddlewareForPath<T>(this WebApplication app, string path) => app.UseWhen(configuration: appBuilder => appBuilder.UseMiddleware<T>(), predicate: context => context.Request.Path.StartsWithSegments(path));

}