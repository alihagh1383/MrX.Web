using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
        public static void StringNullOrWhiteSpace(string P)
        {
            if (string.IsNullOrWhiteSpace(P)) throw new NullParameterException(nameof(P));
        }
        public static void From(ref object P)
        {
            switch (P)
            {
                case null: throw new NullParameterException(nameof(P));
                default: break;
            }
        }
    }
}
