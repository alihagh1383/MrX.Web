using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

public class ValueResult
{
    [MemberNotNullWhen(false, nameof(Error))]
    [MemberNotNullWhen(false, nameof(ErrorName))]
    public bool IsSuccess { get; protected set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public Exception? Error { get; protected set; }

    public string? ErrorName => Error?.GetType().Name;

    protected ValueResult() { }

    // Factory Methods
    public static ValueResult Success() => new() { IsSuccess = true };

    public static ValueResult Failure(Exception exception) => new() { IsSuccess = false, Error = exception };

    public static ValueResult Failure<E>(string? message = null) where E : Exception, new()
    {
        var ex = string.IsNullOrEmpty(message) ? new E() : (E)Activator.CreateInstance(typeof(E), message)!;
        return Failure(ex);
    }

    // Fluent Pattern
    public ValueResult Then(Action action)
    {
        if (!IsSuccess) return this;
        action();
        return this;
    }

    public ValueResult<TOut> Then<TOut>(Func<ValueResult<TOut>> func)
    {
        return IsSuccess ? func() : ValueResult<TOut>.Failure(Error!);
    }

    public async Task<ValueResult> ThenAsync(Func<Task<ValueResult>> func)
    {
        return IsSuccess ? await func() : this;
    }

    public async Task<ValueResult<TOut>> ThenAsync<TOut>(Func<Task<ValueResult<TOut>>> func)
    {
        return IsSuccess ? await func() : ValueResult<TOut>.Failure(Error!);
    }

    public void Match(Action onSuccess, Action<Exception> onFailure)
    {
        if (IsSuccess) onSuccess();
        else onFailure(Error!);
    }
    public static implicit operator ValueResult(Exception ex) => Failure(ex);
}

public class ValueResult<T> : ValueResult
{
    public T? Value { get; protected set; } = default;
    [MemberNotNullWhen(false, nameof(Error))]
    [MemberNotNullWhen(false, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    [MemberNotNullWhen(false, nameof(ErrorName))]
    public new bool IsSuccess { get; protected set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public new Exception? Error { get; protected set; }
    public new string? ErrorName => Error?.GetType().Name;
    protected ValueResult() { }

    // Factory Methods
    public static ValueResult<T> Success(T value) => new() { IsSuccess = true, Value = value };

    public new static ValueResult<T> Failure(Exception exception) => new() { IsSuccess = false, Error = exception };

    public static new ValueResult<T> Failure<E>(string? message = null) where E : Exception, new()
    {
        var ex = string.IsNullOrEmpty(message) ? new E() : (E)Activator.CreateInstance(typeof(E), message)!;
        return Failure(ex);
    }

    // Fluent / Functional helpers
    public ValueResult<TOut> Then<TOut>(Func<T, ValueResult<TOut>> func)
    {
        return IsSuccess ? func(Value!) : ValueResult<TOut>.Failure(Error!);
    }

    public ValueResult Then(Action<T> action)
    {
        if (IsSuccess) action(Value!);
        return this;
    }

    public ValueResult<TOut> Map<TOut>(Func<T, TOut> mapper)
    {
        return IsSuccess ? ValueResult<TOut>.Success(mapper(Value!)) : ValueResult<TOut>.Failure(Error!);
    }

    public async Task<ValueResult<TOut>> ThenAsync<TOut>(Func<T, Task<ValueResult<TOut>>> func)
    {
        return IsSuccess ? await func(Value!) : ValueResult<TOut>.Failure(Error!);
    }

    public void Match(Action<T> onSuccess, Action<Exception> onFailure)
    {
        if (IsSuccess) onSuccess(Value!);
        else onFailure(Error!);
    }

    // Implicit conversions
    public static implicit operator ValueResult<T>(T value) => Success(value);
    public static implicit operator ValueResult<T>(Exception ex) => Failure(ex);
    public static implicit operator T(ValueResult<T> result) => result.IsSuccess ? result.Value! : throw new CannotParseFailureValueResultTToT();
}

public class CannotParseFailureValueResultTToT : Exception { }