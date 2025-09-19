
using MrX.Web.Generics;
using MrX.Web.ValueObject.Common;

namespace MrX.Web.ValueObject
{
    public class Avatar_CanNot_Parse : MrXValueObjectEx;
    public class Avatar_Is_Empty : MrXValueObjectEx;
    public class Avatar
    {
        public static ValueResult<CanNotNull> TryCreate(string? value) =>
            !Guid.TryParse(value, out Guid guid)
                ? new Avatar_CanNot_Parse()
                : TryCreate(guid);
        public static ValueResult<CanNotNull> TryCreate(Guid id) =>
            (id == Guid.Empty)
                ? new Avatar_Is_Empty()
                : new CanNotNull() { Value = id };
        public class CanNull : NullValueObject<CanNull>
        {
            internal CanNull() { }
            public Guid Value { get; internal set; } = Guid.Empty;
            protected override IEnumerable<object?> GetEqualityComponents() => [Value];
            public override string ToString() => IsNull ? "" : Value.ToString();
        }

        public class CanNotNull : NonNullValueObject<CanNull>
        {
            internal CanNotNull() { }
            public Guid Value { get; internal set; } = Guid.Empty;
            public static implicit operator CanNull(CanNotNull obj) => new() { Value = obj.Value };
            protected override IEnumerable<object?> GetEqualityComponents() => [Value];
            public override string ToString() => Value.ToString();
        }
    }
}
