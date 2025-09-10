using System.Diagnostics.CodeAnalysis;

namespace MrX.Web.Generics
{
    public class ValueResult
    {
        [MemberNotNullWhen(false, nameof(Error), nameof(ErrorName))]
        public bool IsSuccess { get; protected set; }

        [Newtonsoft.Json.JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public Exception? Error { get; protected set; }

        public string? ErrorName { get => Error?.GetType().Name; }
        public static ValueResult Success()
        {
            return new ValueResult
            {
                IsSuccess = true,
                Error = null
            };
        }
        public static ValueResult Failure(Exception exception)
        {
            return new ValueResult
            {
                IsSuccess = false,
                Error = exception
            };
        }
        public static ValueResult Failure<E>() where E : Exception
        {
            return new ValueResult
            {
                IsSuccess = false,
                Error = (typeof(E).GetConstructor([])?.Invoke([]) as Exception as E)!
            };
        }
        public static implicit operator ValueResult(Exception exception) => ValueResult.Failure(exception);
    }
    public class ValueResult<T>
    {
        private ValueResult() { }

        [MemberNotNullWhen(false, nameof(Error), nameof(ErrorName))]
        [MemberNotNullWhen(true, nameof(Value))]
        public bool IsSuccess { get; protected set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Exception? Error { get; protected set; }
        public string? ErrorName { get => Error?.GetType().Name; }

        public T? Value { get; protected set; } = default;

        public static ValueResult<T> Success(T value)
        {
            return new ValueResult<T>
            {
                IsSuccess = true,
                Value = value,
                Error = null
            };
        }
        public static ValueResult<T> Failure(Exception exception)
        {
            return new ValueResult<T>
            {
                IsSuccess = false,
                Error = exception
            };
        }
        public static ValueResult<T> Failure<E>() where E : Exception
        {
            return new ValueResult<T>
            {
                IsSuccess = false,
                Error = (typeof(E).GetConstructor([])?.Invoke([]) as Exception as E)!
            };
        }

        public static implicit operator ValueResult<T>(Exception exception) => ValueResult<T>.Failure(exception);
        public static implicit operator ValueResult<T>(T value) => ValueResult<T>.Success(value);
        public static implicit operator T(ValueResult<T> value) => value.Value ?? throw new CannotParsFailureValueResultTToT();
        public static implicit operator ValueResult(ValueResult<T> value) => value.IsSuccess ? ValueResult.Success() : ValueResult.Failure(value.Error);
        public static implicit operator ValueResult<T>(ValueResult valueResult) =>
            (valueResult.IsSuccess)
            ? new() { Error = new CannotParsSucceedValueResultToValueResultT(), IsSuccess = false }
            : new() { Error = valueResult.Error, IsSuccess = valueResult.IsSuccess, Value = default };
    }

    public class CannotParsSucceedValueResultToValueResultT : Exception;
    public class CannotParsFailureValueResultTToT : Exception;
}
