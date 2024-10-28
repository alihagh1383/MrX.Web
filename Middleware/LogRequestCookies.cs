﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MrX.Web.Middleware
{
    public class LogRequestCookies 
    {
        private readonly RequestDelegate next;
        public LogRequestCookies(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Items["Log"] is SetupLogMiddleware.Log L)
            {
                L.Request_Cookies = context.Request.Cookies.ToDictionary();
                await next.Invoke(context);
            }
            else
                throw new Exception("SetupLogMiddleware Not Found");

        }
    }
}