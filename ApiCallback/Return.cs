using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace MrX.Web.ApiCallback
{
    public class Return<T>
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        protected Return(bool success, int status, string? message = null, T? data = default)
        {
            IsSuccess = success;
            StatusCode = status;
            Message = message;
            Data = data;
        }

        public void WriteToHttpContext(HttpContext httpContext) => WriteToHttpResponse(httpContext.Response);
        public void WriteToHttpResponse(HttpResponse response)
        {
            response.StatusCode = StatusCode;
            response.WriteAsJsonAsync<Return<T>>(this);
        }
        public IResult ToResult() => Results.Json(this, statusCode: StatusCode);
        public IActionResult ToActionResult() => new JsonResult(this) { StatusCode = StatusCode };

        /// <summary>
        ///     409
        /// </summary>
        public static Return<T> ThisExist(string? message, T? data = default)
        {
            return new Return<T>(false, 409, message, data);
        }

        /// <summary>
        ///     403
        /// </summary>
        public static Return<T> AccessDeny(string? message, T? data = default)
        {
            return new Return<T>(false, 403, message, data);
        }

        /// <summary>
        ///     200
        /// </summary>
        public static Return<T> Sucsses(bool success, string? message, T? data = default)
        {
            return new Return<T>(success, 200, message, data);
        }

        /// <summary>
        ///     404
        /// </summary>
        public static Return<T> NotExist(string message, T? data = default)
        {
            return new Return<T>(false, 404, message, data);
        }

        /// <summary>
        ///     400
        /// </summary>
        public static Return<T> HeaderNotFound(string message, T? data = default)
        {
            return new Return<T>(false, 400, message, data);
        }

        /// <summary>
        ///     400
        /// </summary>
        public static Return<T> Invalid(string message, T? data = default)
        {
            return new Return<T>(false, 400, message, data);
        }

        /// <summary>
        ///     404
        /// </summary>
        public static Return<T> NotFound(string message, T? data = default)
        {
            return new Return<T>(false, 404, message, data);
        }

        /// <summary>
        ///     400
        /// </summary>
        public static Return<T> ParameterNotFound(string message, T? data = default)
        {
            return new Return<T>(false, 400, message, data);
        }

        /// <summary>
        ///     419
        /// </summary>
        public static Return<T> Expire(string message, T? data = default)
        {
            return new Return<T>(false, 419, message, data);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented,
                new StringEnumConverter());
        }
    }
    public class Return(bool success, int status, string? message = null, object? data = null)
        : Return<object?>(success, status, message, data);
}