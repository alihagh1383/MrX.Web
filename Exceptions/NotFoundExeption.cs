using Microsoft.AspNetCore.Http;

namespace MrX.Web.Exceptions;

public class NotFoundExeption : ApiException
{
    public NotFoundExeption(object data)
    {
        Message = $"The requested resource was not found: {data}";
        StatusCode = StatusCodes.Status404NotFound;
        base.Data = data;
    }
}