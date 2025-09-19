using MrX.Web.Generics;
using MrX.Web.ValueObject.Common;

namespace MrX.Web.ValueObject
{
    public class Role_Is_Empty : MrXValueObjectEx;

    public class Role : NonNullValueObject<Role>
    {
        public string Value { get; private set; } = null!;
        private Role() { }
        public static ValueResult<Role> TryCreate(string value) =>
            (string.IsNullOrWhiteSpace(value))
                ? new Role_Is_Empty()
                : new Role { Value = value };
        public override string ToString() => Value;
        protected override IEnumerable<object?> GetEqualityComponents() => [Value];
    }
}
