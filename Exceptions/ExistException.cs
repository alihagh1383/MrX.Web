using Microsoft.AspNetCore.Http;

namespace MrX.Web.Exceptions
{
    public class ExistException : ApiException
    {
        public ExistException(string @for, object data)
        {
            Message = $"This Exist";
            StatusCode = StatusCodes.Status412PreconditionFailed;
            data = new { For = @for, Data = data };
        }
    }
}
