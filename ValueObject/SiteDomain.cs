using MrX.Web.ValueObject.Common;

namespace MrX.Web.ValueObject
{
    public class SiteDomain_Is_Empty : MrXValueObjectEx;
    public class SiteDomain_Is_Not_Valid : MrXValueObjectEx;

    public class SiteDomain
    {
        public static ValueResult<CanNotNull> TryCreate(string value) =>
          (string.IsNullOrWhiteSpace(value))
              ? new Url_Is_Empty()
              : (!Uri.TryCreate(value, UriKind.Absolute, out Uri? u))
                  ? new Url_Is_Not_Valid()
                  : (!Uri.TryCreate(u.Scheme + "://" + u.Authority, UriKind.Absolute, out u))
                      ? new Url_Is_Not_Valid()
                      : new CanNotNull() { Value = new Uri($"{u.Scheme}://{u.Host}") };
        public class CanNull : NullValueObject<CanNull>
        {
            public Uri? Value { get; internal init; }
            internal CanNull() { }
            protected override IEnumerable<object?> GetEqualityComponents() => [Value];
            public override string ToString() => IsNull ? "" : Value?.ToString() ?? "";
        }
        public class CanNotNull : NonNullValueObject<CanNull>
        {
            internal CanNotNull() { }
            public Uri Value { get; internal init; } = null!;
            public static implicit operator CanNull(CanNotNull obj) => new()
            {
                Value = obj.Value
            };
            protected override IEnumerable<object?> GetEqualityComponents() => [Value];
            public override string ToString() => Value.ToString();
        }

    }
}
