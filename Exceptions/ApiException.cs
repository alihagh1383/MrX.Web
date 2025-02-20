using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MrX.Web.ApiCallback;

namespace MrX.Web.Exceptions
{
    public abstract class ApiException() : Exception
    {
        public Boolean IsSuccess { get; set; } = false;
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public Return ToReturn() => new Return(IsSuccess, StatusCode, Message, Data);
    }
}
