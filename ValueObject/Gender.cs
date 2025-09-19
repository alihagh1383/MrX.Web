using MrX.Web.Generics;
using MrX.Web.ValueObject.Common;

namespace MrX.Web.ValueObject
{
    public class Gender_Not_Valid : MrXValueObjectEx;

    public class Gender
    {
        public static ValueResult<CanNotNull> TryCreate(string Value) =>
            (!Enum.TryParse<Genders>(Value, out Genders r))
                ? new Gender_Not_Valid()
                : new CanNotNull { Value = r };

        public static ValueResult<CanNotNull> TryCreate(Genders Value) =>
            new CanNotNull { Value = Value };
        public enum Genders { Woman = 0b10, Man = 0b1, }
        public class CanNull : NullValueObject<CanNull>
        {
            internal CanNull() { }
            public Genders Value { get; internal init; }
            protected override IEnumerable<object?> GetEqualityComponents() => [Value];
            public override string ToString() => IsNull ? "" : Value.ToString();
        }
        public class CanNotNull : NonNullValueObject<CanNotNull>
        {
            internal CanNotNull() { }
            public Genders Value { get; internal init; }
            public static implicit operator CanNull(CanNotNull obj) => new() { Value = obj.Value };
            protected override IEnumerable<object?> GetEqualityComponents() => [Value];
            public override string ToString() => Value.ToString();
        }
    }
}
