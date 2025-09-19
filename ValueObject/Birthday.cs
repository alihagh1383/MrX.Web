using MrX.Web.Generics;
using MrX.Web.ValueObject.Common;

namespace MrX.Web.ValueObject
{
    public class Birthday_Is_For_Future : MrXValueObjectEx;
    public class Birthday_Format_Is_Not_Valid : MrXValueObjectEx;

    public class Birthday
    {
        public static ValueResult<CanNotNull> TryCreate(string value) =>
            (!DateOnly.TryParse(value, out DateOnly r))
                ? new Birthday_Format_Is_Not_Valid()
                : TryCreate(r);
        public static ValueResult<CanNotNull> TryCreate(DateOnly value) =>
            (DateOnly.FromDateTime(DateTime.UtcNow) <= value)
                ? new Birthday_Is_For_Future()
                : new CanNotNull { Value = value };
        public class CanNull : NullValueObject<CanNull>
        {
            internal CanNull() { }
            public DateOnly Value { get; internal set; } = DateOnly.MinValue;
            protected override IEnumerable<object?> GetEqualityComponents() => [Value];
            public override string ToString() => IsNull ? "" : $"{Value.Year}/{Value.Month}/{Value.Day}";
        }

        public class CanNotNull : NonNullValueObject<CanNull>
        {
            internal CanNotNull() { }
            public DateOnly Value { get; internal set; } = DateOnly.MinValue;
            public static implicit operator CanNull(CanNotNull obj) => new() { Value = obj.Value };
            protected override IEnumerable<object?> GetEqualityComponents() => [Value];
            public override string ToString() => $"{Value.Year}/{Value.Month}/{Value.Day}";
        }
    }
}
