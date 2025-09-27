using MrX.Web.ValueObject.Common;

namespace MrX.Web.ValueObject
{
    public class Family_Is_Empty : MrXValueObjectEx;
    public class Family
    {
        public static ValueResult<CanNotNull> TryCreate(string? value) =>
            (string.IsNullOrWhiteSpace(value))
                ? new Family_Is_Empty()
                : new CanNotNull { Value = value };

        public class CanNull : NullValueObject<CanNull>
        {
            internal CanNull() { }
            public string Value { get; internal init; } = string.Empty;
            protected override IEnumerable<object?> GetEqualityComponents() => [Value];
            public override string ToString() => IsNull ? "" : $"{Value}";
        }

        public class CanNotNull : NonNullValueObject<CanNull>
        {
            internal CanNotNull() { }
            public string Value { get; internal init; } = string.Empty;
            public static implicit operator CanNull(CanNotNull obj) => new() { Value = obj.Value };
            protected override IEnumerable<object?> GetEqualityComponents() => [Value];
            public override string ToString() => $"{Value}";
        }
    }
}
