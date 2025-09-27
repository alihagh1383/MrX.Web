using MrX.Web.Security;
using MrX.Web.ValueObject.Common;

namespace MrX.Web.ValueObject;

public class Password_Is_Empty : MrXValueObjectEx;

public class Password_Is_Lang_Or_Short(int min, int max) : MrXValueObjectEx
{
    private readonly int min = min;
    private readonly int max = max;
}

public class Password
{
    public static ValueResult<CanNotNull> TryCreate(string password) =>
        (string.IsNullOrWhiteSpace(password))
            ? new Password_Is_Empty()
            : (password.Length is < 8 or > 100)
                ? new Password_Is_Lang_Or_Short(8, 100)
                : new CanNotNull() { Value = PasswordHash.Hash(password) };
    public static ValueResult<CanNotNull> TryCreateFromHash(string password) =>
        (string.IsNullOrWhiteSpace(password))
            ? new Password_Is_Empty()
            : new CanNotNull() { Value = (password) };
    public class CanNull : NullValueObject<CanNull>
    {
        internal CanNull() { }
        public string Value { get; internal init; } = string.Empty;
        public bool Check(string password) => !IsNull && PasswordHash.Verify(password, Value);
        protected override IEnumerable<object?> GetEqualityComponents() => [Value];
        public override string ToString() => IsNull ? "" : Value.ToString();
    }
    public class CanNotNull : NonNullValueObject<CanNotNull>
    {
        internal CanNotNull() { }
        public string Value { get; internal init; } = string.Empty;
        public static implicit operator CanNull(CanNotNull obj) => new() { Value = obj.Value };
        public bool Check(string password) => PasswordHash.Verify(password, Value);
        protected override IEnumerable<object?> GetEqualityComponents() => [Value];
        public override string ToString() => Value.ToString();
    }
}