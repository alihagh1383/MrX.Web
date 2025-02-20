using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace MrX.Web.ApiCallback
{
    public class Return
    {
        public Boolean IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public Return(bool success, int status, string? message = null, object? data = null)
        {
            IsSuccess = success;
            StatusCode = status;
            Message = message;
            Data = data;
        }
        /// <summary>
        ///     409
        /// </summary>
        public static Return ThisExist(string? message, object? data = null)
        {
            return new(false, 409, message, data);
        }
        /// <summary>
        ///     403
        /// </summary>
        public static Return AccessDeny(string? message, object? data = null)
        {
            return new(false, 403, message, data);
        }
        /// <summary>
        ///     200
        /// </summary>
        public static Return Sucsses(bool success, string? message, object? data = null)
        {
            return new(success, 200, message, data);
        }
        /// <summary>
        ///     404
        /// </summary>
        public static Return NotExist(string Message, object? data = null)
        {
            return new(false, 404, Message, data);
        }

        /// <summary>
        ///     400
        /// </summary>
        public static Return HeaderNotFound(string Message, object? data = null)
        {
            return new(false, 400, Message, data);
        }

        /// <summary>
        ///     400
        /// </summary>
        public static Return Invalid(string Message, object? data = null)
        {
            return new(false, 400, Message, data);
        }

        /// <summary>
        ///     404
        /// </summary>
        public static Return NotFound(string Message, object? data = null)
        {
            return new(false, 404, Message, data);
        }

        /// <summary>
        ///     400
        /// </summary>
        public static Return ParameterNotFound(string Message, object? data = null)
        {
            return new(false, 400, Message, data);
        }

        /// <summary>
        ///     419
        /// </summary>
        public static Return Expire(string Message, object? data = null)
        {
            return new(false, 419, Message, data);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented,
                new StringEnumConverter());
        }
    }
}
