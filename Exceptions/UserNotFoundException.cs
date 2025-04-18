﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MrX.Web.Exceptions
{
    public class UserNotFoundException : ApiException
    {
        public UserNotFoundException(string username, string password)
        {
            Message = "User Not Found";
            StatusCode = StatusCodes.Status404NotFound;
            Data = new { username, password };
        }
    }
}
