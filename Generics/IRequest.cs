using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MrX.Web.Generics;

public interface ICommand;
public interface ICommandHandler<T> where T : ICommand
{
    public Task<ValueResult> Invoke(T request, CancellationToken cancellation);
}
/////////////////////////////////////////////////////////////////////////////
public interface ICommandResult<R>;
public interface ICommandResultHandler<T, R> where T : ICommandResult<R>
{
    public Task<ValueResult<R>> Invoke(T request, CancellationToken cancellation);
}
/////////////////////////////////////////////////////////////////////////////
public interface IQuery;
public interface IQueryHandler<T> where T : IQuery
{
    public Task<ValueResult> Invoke(T request, CancellationToken cancellation);
}
/////////////////////////////////////////////////////////////////////////////
public interface IQueryResult<R>;
public interface IQueryResultHandler<T, R> where T : IQueryResult<R>
{
    public Task<ValueResult<R>> Invoke(T request, CancellationToken cancellation);
}
public static class CommandAndQueryRegisteryExtensions
{
    internal static Dictionary<Type, Type> Queres = [];
    internal static Dictionary<Type, Type> Commands = [];
    internal static Dictionary<Type, Type> RQueres = [];
    internal static Dictionary<Type, Type> RCommands = [];
    extension(IServiceCollection services)
    {
        public void AddCommandAndQueryRegistery(params List<Assembly> assemblys)
        {
            services.AddScoped<CommandAndQueryRegistery>();
            foreach (Assembly assembly in assemblys)
            {
                foreach (Type handlerType in assembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<>))))
                    foreach (Type handlerInterface in handlerType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<>)))
                    {
                        Type requestType = handlerInterface.GetGenericArguments()[0];
                        Type serviceType = typeof(IQueryHandler<>).MakeGenericType(requestType);
                        services.AddTransient(serviceType, handlerType);
                    }
                foreach (Type handlerType in assembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryResultHandler<,>))))
                    foreach (Type handlerInterface in handlerType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryResultHandler<,>)))
                    {
                        Type requestType = handlerInterface.GetGenericArguments()[0];
                        Type resultType = handlerInterface.GetGenericArguments()[1];
                        Type serviceType = typeof(IQueryResultHandler<,>).MakeGenericType(requestType, resultType);
                        services.AddTransient(serviceType, handlerType);
                    }

                foreach (Type handlerType in assembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>))))
                    foreach (Type handlerInterface in handlerType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)))
                    {
                        Type requestType = handlerInterface.GetGenericArguments()[0];
                        Type serviceType = typeof(ICommandHandler<>).MakeGenericType(requestType);
                        services.AddTransient(serviceType, handlerType);
                    }
                foreach (Type handlerType in assembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandResultHandler<,>))))
                    foreach (Type handlerInterface in handlerType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandResultHandler<,>)))
                    {
                        Type requestType = handlerInterface.GetGenericArguments()[0];
                        Type resultType = handlerInterface.GetGenericArguments()[1];
                        Type serviceType = typeof(ICommandResultHandler<,>).MakeGenericType(requestType, resultType);
                        services.AddTransient(serviceType, handlerType);
                    }
            }
        }
    }
}
public class CommandAndQueryRegistery(IServiceProvider service)
{
    public async Task<ValueResult<object>> SendWhitResult(object message, CancellationToken cancellation)
    {
        switch (message)
        {
            case IQueryResult<object> query:
                {
                    Type handlerT = CommandAndQueryRegisteryExtensions.RQueres[query.GetType()];
                    object? handler = service.GetService(handlerT);
                    return handler is not { } h
                        ? (ValueResult<object>)new ArgumentException("Type Of message not Support")
                        : await (h as IQueryResultHandler<IQueryResult<object>, object>)!.Invoke((message as IQueryResult<object>)!, cancellation);
                }
            case ICommandResult<object> query:
                {
                    Type handlerT = CommandAndQueryRegisteryExtensions.RCommands[query.GetType()];
                    object? handler = service.GetService(handlerT);
                    return handler is not { } h
                        ? (ValueResult<object>)new ArgumentException("Type Of message not Support")
                        : await (h as ICommandResultHandler<ICommandResult<object>, object>)!.Invoke((message as ICommandResult<object>)!, cancellation);
                }
            default:
                return new ArgumentException("Type Of message not Support");
        }
    }
    public async Task<ValueResult> SendWhitOutResult(object message, CancellationToken cancellation)
    {
        switch (message)
        {
            case IQuery query:
                {
                    Type handlerT = CommandAndQueryRegisteryExtensions.Queres[query.GetType()];
                    object? handler = service.GetService(handlerT);
                    return handler is not { } h
                        ? (ValueResult)new ArgumentException("Type Of message not Support")
                        : await (h as IQueryHandler<IQuery>)!.Invoke((message as IQuery)!, cancellation);
                }
            case IQueryResult<object> query:
                {
                    Type handlerT = CommandAndQueryRegisteryExtensions.RQueres[query.GetType()];
                    object? handler = service.GetService(handlerT);
                    if (handler is not { } h) return new ArgumentException("Type Of message not Support");
                    ValueResult<object> r = await (h as IQueryResultHandler<IQueryResult<object>, object>)!.Invoke((message as IQueryResult<object>)!, cancellation);
                    return r.IsSuccess
                        ? ValueResult.Success()
                        : ValueResult.Failure(r.Error);
                }
            case ICommand command:
                {
                    Type handlerT = CommandAndQueryRegisteryExtensions.Commands[command.GetType()];
                    object? handler = service.GetService(handlerT);
                    return handler is not { } h
                        ? (ValueResult)new ArgumentException("Type Of message not Support")
                        : await (h as ICommandHandler<ICommand>)!.Invoke((message as ICommand)!, cancellation);
                }
            case ICommandResult<object> query:
                {
                    Type handlerT = CommandAndQueryRegisteryExtensions.RCommands[query.GetType()];
                    object? handler = service.GetService(handlerT);
                    if (handler is not { } h) return new ArgumentException("Type Of message not Support");
                    ValueResult<object> r = (await (h as ICommandResultHandler<ICommandResult<object>, object>)!.Invoke((message as ICommandResult<object>)!, cancellation));
                    return r.IsSuccess
                        ? ValueResult.Success()
                        : ValueResult.Failure(r.Error);
                }
            default: return new ArgumentException("Type Of message not Support");
        }

    }
}