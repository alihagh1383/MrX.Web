using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MrX.Web.ApiCallback;
using MrX.Web.Exceptions;
using Newtonsoft.Json;

namespace MrX.Web.Middleware
{
    public class ApiExceptionHandler(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                if (ex is ApiException aex)
                {
                    httpContext.Response.StatusCode = aex.StatusCode;
                    httpContext.Response.WriteAsync(aex.ToReturn().ToString(),Encoding.UTF8).Wait();
                }
                else { throw; }
            }
        }
    }
}
