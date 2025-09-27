using MrX.Web.ValueObject.Common;

namespace MrX.Web.ValueObject
{
    public class ApiKey_Len_Most_Be_256 : MrXValueObjectEx;
    public class Apikey
    {
        public static ValueResult<CanNotNull> TryCreate(string? value) =>
            !string.IsNullOrWhiteSpace(value) || value?.Length != 256
                ? new ApiKey_Len_Most_Be_256()
                : new CanNotNull { Value = value };
        public static CanNotNull New() => new() { Value = MrX.Web.Security.Random.String(256, true, false, true) };
        public class CanNull : NullValueObject<CanNull>
        {
            internal CanNull() { }
            public string Value { get; internal init { field = value.ToLower(); } } = string.Empty;
            protected override IEnumerable<object?> GetEqualityComponents() => [Value];
            public override string ToString() => IsNull ? "" : Value.ToString();
        }

        public class CanNotNull : NonNullValueObject<CanNotNull>
        {
            internal CanNotNull() { }
            public string Value { get; internal init { field = value.ToLower(); } } = string.Empty;
            public static implicit operator CanNull(CanNotNull obj) => new() { Value = obj.Value };
            protected override IEnumerable<object?> GetEqualityComponents() => [Value];
            public override string ToString() => Value.ToString();

        }
    }
}
