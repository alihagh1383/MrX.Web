using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
