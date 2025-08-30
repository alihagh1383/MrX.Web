using Microsoft.AspNetCore.Http;
using MrX.Web.Logger;
using Newtonsoft.Json;
using System.Collections;

namespace MrX.Web.Middleware;

/// <summary>
///     ADD SecurityLogger to Services Required
///     add in context.Items Key "Log" and Value MrX.Web.Middleware.SetupLogMiddleware.Log
/// </summary>
public class SetupLogMiddleware(RequestDelegate next, SecurityLogger logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();
        Stream org = context.Response.Body;
        MemoryStream memoryStream = new();
        context.Response.Body = memoryStream;
        context.Items.Add("Log", new Log(logger, context.Connection.Id)
        {
            RequestFrom =
                $"{context.Connection.Id} : {context.Connection.RemoteIpAddress}:{context.Connection.RemotePort} > {context.Connection.LocalIpAddress}:{context.Connection.LocalPort}"
        });
        await next.Invoke(context);
        memoryStream.Position = 0;
        await memoryStream.CopyToAsync(org);
        context.Response.Body = org;
        (context.Items["Log"] as Log)!.DoLog();
    }

    public class Log(SecurityLogger logger, string connectionId)
    {
        public string ConnectionId { get; set; } = connectionId;
        public string RequestMethod { get; set; } = string.Empty;
        public string RequestProtocol { get; set; } = string.Empty;
        public string RequestQuery { get; set; } = string.Empty;
        public ICollection RequestCookies { get; set; } = new Dictionary<string, string>();
        public string RequestBody { get; set; } = string.Empty;
        public IHeaderDictionary RequestHeaders { get; set; } = new HeaderDictionary();
        public string RequestFrom { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string ResponseBody { get; set; } = string.Empty;
        public IHeaderDictionary ResponseHeaders { get; set; } = new HeaderDictionary();
        public string Message { get; set; } = string.Empty;
        public string RequestPath { get; set; } = string.Empty;


        public void DoLog()
        {
            logger.Log(ConnectionId, this);
        }

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
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}