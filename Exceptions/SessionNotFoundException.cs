using Microsoft.AspNetCore.Http;

namespace MrX.Web.Exceptions
{
    public class SessionNotFoundException : ApiException
    {
        public SessionNotFoundException(object sessionId)
        {
            Message = "Session Not Found";
            StatusCode = StatusCodes.Status404NotFound;
            Data = new { SessionId = sessionId };
        }
    }
}
