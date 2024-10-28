using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace MrX.Web.Middleware
{
    /// <summary>
    /// add in context.Items Key "Log" and Value MrX.Web.Middleware.SetupLogMiddleware.Log
    /// </summary>
    public class SetupLogMiddleware
    {
        private readonly RequestDelegate next;
        Action<Log> DoLog;
        public SetupLogMiddleware(RequestDelegate next, Action<Log> DoLog)
        {
            this.next = next;
            this.DoLog = DoLog;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            var org = context.Response.Body;
            var MemoryStream = new MemoryStream();
            context.Response.Body = MemoryStream;
            context.Items.Add("Log", new Log(DoLog)
            {
                Request_From = $"{context.Connection.Id} : {context.Connection.RemoteIpAddress}:{context.Connection.RemotePort} > {context.Connection.LocalIpAddress}:{context.Connection.LocalPort}"
            });
            await next.Invoke(context);
            MemoryStream.Position = 0;
            await MemoryStream.CopyToAsync(org);
            context.Response.Body = org;
            (context.Items["Log"] as Log)!.DoLog();
        }
        public class Log
        {
            private Action<Log> doLog;
            public Log(Action<Log> doLog)
            {
                this.doLog = doLog;
            }
            public void DoLog() => doLog.Invoke(this);

            public string Request_Metod { get; set; } = string.Empty;
            public string Request_Path { get; set; } = string.Empty;
            public string Request_Protocol { get; set; } = string.Empty;
            public string Request_Query { get; set; } = string.Empty;
            public ICollection Request_Cookies { get; set; } = new Dictionary<string,string>();
            public string Request_Body { get; set; } = string.Empty;
            public IHeaderDictionary Request_Headers { get; set; } =new HeaderDictionary();
            public string Request_From { get; set; } = string.Empty;
            public string User { get; set; } = string.Empty;
            public string Response_Body { get; set; } = string.Empty;
            public IHeaderDictionary Response_Headers { get; set; } = new HeaderDictionary();
            public string Message { get; set; } = string.Empty;
            public override string ToString()
            {
                //return $"""
                //    ----------------------------------------------------------------------
                //    {Request_From} 
                //    {User}
                //    ----------------------------------------------------------------------
                //    -- Request
                //    {Request_Metod} {Request_Path} {Request_Protocol} ? {Request_Query}
                //    {Request_Headers}

                //    {Request_Body}
                //    -- Request Cookies 
                //    {Request_Cookies}
                //    ----------------------------------------------------------------------
                //    -- Response
                //    {Response_Headers}

                //    {Response_Body}
                //    ----------------------------------------------------------------------
                //    """;
                return Newtonsoft.Json.JsonConvert.SerializeObject(this,Newtonsoft.Json.Formatting.Indented);
            }
        }
    }
}
