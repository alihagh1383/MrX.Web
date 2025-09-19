using System.ComponentModel.DataAnnotations;
using Auth.Ex;
using MrX.Web.Generics;
using MrX.Web.ValueObject.Common;

namespace MrX.Web.ValueObject;

public class Email_Is_Empty : ValueObjectEx;

public class Email_Is_Not_Valid : ValueObjectEx;
public abstract class Email
{
    public static ValueResult<CanNotNull> TryCreate(string? value) =>
        (string.IsNullOrWhiteSpace(value))
            ? new Email_Is_Empty()
            : (!new EmailAddressAttribute().IsValid(value))
                ? new Email_Is_Not_Valid()
                : new CanNotNull { Value = value, IsVerified = false };
    public class CanNull : NullValueObject<CanNull>
    {
        internal CanNull() { }
        public string Value { get; internal init; } = string.Empty;
        public bool IsVerified { get; internal set; } = false;
        public void Verify() => IsVerified = true;
        protected override IEnumerable<object?> GetEqualityComponents() => [Value];
        public override string ToString() => IsNull ? "" : Value;
    }

    public class CanNotNull : NonNullValueObject<CanNull>
    {
        internal CanNotNull() { }
        public string Value { get; internal init; } = null!;
        public bool IsVerified { get; internal set; } = false;
        public void Verify() => IsVerified = true;
        public static implicit operator CanNull(CanNotNull obj) => new() { Value = obj.Value, IsVerified = obj.IsVerified };
        protected override IEnumerable<object?> GetEqualityComponents() => [Value];
        public override string ToString() => Value;
    }
}