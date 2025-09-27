using MrX.Web.ValueObject.Common;

namespace MrX.Web.ValueObject;

public class Id_CanNot_Parse : MrXValueObjectEx;
public class Id_Is_Empty : MrXValueObjectEx;
public class Id
{
    public static ValueResult<CanNotNull> TryCreate(string? value) =>
        !Guid.TryParse(value, out Guid guid)
            ? new Id_CanNot_Parse()
            : TryCreate(guid);
    public static ValueResult<CanNotNull> TryCreate(Guid id) =>
        (id == Guid.Empty)
            ? new Id_Is_Empty()
            : new CanNotNull() { Value = id };
    public static CanNotNull New() => new() { Value = Guid.CreateVersion7() };
    public class CanNull : NullValueObject<CanNull>
    {
        internal CanNull() { }
        public Guid Value { get; internal init; }
        protected override IEnumerable<object?> GetEqualityComponents() => [Value];
        public override string ToString() => IsNull ? "" : Value.ToString();
    }

    public class CanNotNull : NonNullValueObject<CanNotNull>
    {
        internal CanNotNull() { }
        public Guid Value { get; internal init; }
        public static implicit operator CanNull(CanNotNull obj) => new() { Value = obj.Value };
        protected override IEnumerable<object?> GetEqualityComponents() => [Value];
        public override string ToString() => Value.ToString();

    }
}