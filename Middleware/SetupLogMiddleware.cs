using System.Collections;
using Microsoft.AspNetCore.Http;
using MrX.Web.Logger;

namespace MrX.Web.Middleware
{
    /// <summary>
    /// ADD SecurityLogger to Services Required 
    /// add in context.Items Key "Log" and Value MrX.Web.Middleware.SetupLogMiddleware.Log
    /// </summary>
    public class SetupLogMiddleware(RequestDelegate next, SecurityLogger logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            var org = context.Response.Body;
            var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;
            context.Items.Add("Log", new Log(logger,context.Connection.Id)
            {
                Request_From =
                    $"{context.Connection.Id} : {context.Connection.RemoteIpAddress}:{context.Connection.RemotePort} > {context.Connection.LocalIpAddress}:{context.Connection.LocalPort}"
            });
            await next.Invoke(context);
            memoryStream.Position = 0;
            await memoryStream.CopyToAsync(org);
            context.Response.Body = org;
            (context.Items["Log"] as Log)!.DoLog();
        }

        public class Log(SecurityLogger logger,string connectionId)
        {
           

            public void DoLog() => logger.Log(Connection_Id, this, true);
            public string Connection_Id { get; set; } = connectionId;
            public string Request_Method { get; set; } = string.Empty;
            public string Request_Protocol { get; set; } = string.Empty;
            public string Request_Query { get; set; } = string.Empty;
            public ICollection Request_Cookies { get; set; } = new Dictionary<string, string>();
            public string Request_Body { get; set; } = string.Empty;
            public IHeaderDictionary Request_Headers { get; set; } = new HeaderDictionary();
            public string Request_From { get; set; } = string.Empty;
            public string User { get; set; } = string.Empty;
            public string Response_Body { get; set; } = string.Empty;
            public IHeaderDictionary Response_Headers { get; set; } = new HeaderDictionary();
            public string Message { get; set; } = string.Empty;
            public string Request_Path { get; set; } = string.Empty;

            public override string ToString()
            {
                //return $"""
                //    ----------------------------------------------------------------------
                //    {Request_From} 
                //    {User}
                //    ----------------------------------------------------------------------
                //    -- Request
                //    {Request_Method} {Request_Path} {Request_Protocol} ? {Request_Query}
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
                return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
            }
        }
    }
}