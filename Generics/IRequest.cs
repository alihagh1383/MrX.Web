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