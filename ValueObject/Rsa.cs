using MrX.Web.ValueObject.Common;
using System.Security.Cryptography;

namespace MrX.Web.ValueObject
{
    public class Rsa_Is_Empty : MrXValueObjectEx;
    public class Rsa_Is_Not_Valid : MrXValueObjectEx;

    public class Rsa
    {
        public static ValueResult<CanNotNull> TryCreate(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return new Rsa_Is_Empty();
            RSA rsa = RSA.Create();
            try { rsa.ImportFromPem(value); }
            catch { return new Rsa_Is_Not_Valid(); }
            return new CanNotNull() { Value = value };
        }
        public static ValueResult<CanNotNull> TryCreate(RSA value) =>
            (value.KeySize == 0)
                ? new Rsa_Is_Not_Valid()
                : TryCreate(value.ExportRSAPrivateKeyPem());
        public static CanNotNull New() => new() { Value = RSA.Create(2048).ExportRSAPrivateKeyPem() };
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
}
