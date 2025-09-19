using System.Text.RegularExpressions;
using MrX.Web.Generics;
using MrX.Web.ValueObject.Common;

public class National_Code_Not_Valid : MrXValueObjectEx;
namespace MrX.Web.ValueObject
{
    public partial class NationalCode
    {
        public static ValueResult<CanNotNull> TryCreate(string value) =>
            (!IsValidIranianNationalCode(value))
                ? new National_Code_Not_Valid()
                : new CanNotNull { Value = value };

        public static bool IsValidIranianNationalCode(string input)
        {
            if (!len().IsMatch(input))
                return false;

            int check = Convert.ToInt32(input.Substring(9, 1));
            int sum = Enumerable.Range(0, 9)
                .Select(x => Convert.ToInt32(input.Substring(x, 1)) * (10 - x))
                .Sum() % 11;

            return sum < 2 ? check == sum : check + sum == 11;
        }
        public class CanNull : NullValueObject<CanNull>
        {
            internal CanNull() { }
            public string Value { get; internal init; } = string.Empty;
            protected override IEnumerable<object?> GetEqualityComponents() => [Value];
            public override string ToString() => IsNull ? "" : Value;
        }
        public class CanNotNull : ValueObject<CanNotNull>
        {
            internal CanNotNull() { }
            public string Value { get; internal init; } = string.Empty;
            public static implicit operator CanNull(CanNotNull obj) => new() { Value = obj.Value };
            protected override IEnumerable<object?> GetEqualityComponents() => [Value];
            public override string ToString() => Value;
        }

        [GeneratedRegex(@"^\d{10}$")]
        public static partial Regex len();
    }
}
