using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
