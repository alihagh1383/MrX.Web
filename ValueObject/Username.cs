using MrX.Web.ValueObject.Common;

namespace MrX.Web.ValueObject;

public class UserName_Is_Empty : MrXValueObjectEx;
public class UserName_Is_Lang_Or_Short(int min, int max) : MrXValueObjectEx
{
    private readonly int min = min;
    private readonly int max = max;
}

public class Username
{
    public static ValueResult<CanNotNull> TryCreate(string? value) =>
        (string.IsNullOrWhiteSpace(value))
            ? new UserName_Is_Empty()
            : (value.Length is < 8 or > 100)
                ? new UserName_Is_Lang_Or_Short(8, 100)
                : new CanNotNull { Value = value };
    public class CanNull : NullValueObject<CanNull>
    {
        internal CanNull() { }
        public string Value { get; internal init { field = value.ToLower(); } } = string.Empty;
        protected override IEnumerable<object?> GetEqualityComponents() => [Value];
        public override string ToString() => IsNull ? "" : Value;
    }
    public class CanNotNull : NonNullValueObject<CanNull>
    {
        internal CanNotNull() { }
        public string Value { get; internal init { field = value.ToLower(); } } = string.Empty;
        public static implicit operator CanNull(CanNotNull obj) => new() { Value = obj.Value };
        protected override IEnumerable<object?> GetEqualityComponents() => [Value];
        public override string ToString() => Value;
    }
}