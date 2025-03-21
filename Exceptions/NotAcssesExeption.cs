using System.Net;
using Microsoft.AspNetCore.Http;

namespace MrX.Web.Exceptions;

public class NotAcssesExeption : ApiException
{
    public NotAcssesExeption(object data)
    {
        Message = "Access is denied To This Resource";
        data = data;
        StatusCode = StatusCodes.Status403Forbidden;
    }
}