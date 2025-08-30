using Microsoft.AspNetCore.Http;

namespace MrX.Web.Exceptions
{
    public class NullParameterException : ApiException
    {
        public NullParameterException(params List<string>? s)
        {
            Message = $"Null Parameter Error {((s is not null) ? "For " + s : "")}";
            StatusCode = StatusCodes.Status400BadRequest;
            Data = new { Parameters = s };
        }
        public static NullParameterException? StringNullOrWhiteSpace(string p)
        {
            if (string.IsNullOrWhiteSpace(p)) return new NullParameterException(nameof(p));
            return null;
        }
        public static NullParameterException? From(ref object p)
        {
            switch (p)
            {
                case null: return new NullParameterException(nameof(p));
                default: return null;
            }

        }
    }
}
