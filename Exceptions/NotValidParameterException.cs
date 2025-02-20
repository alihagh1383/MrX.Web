using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MrX.Web.Exceptions
{
    public class NotValidParameterException : ApiException
    {
        public NotValidParameterException(Type type, string data)
        {
            Message = $"Not Valid Parameter";
            StatusCode = StatusCodes.Status400BadRequest;
            Data = new { ValidType = type.Name, SendedParameter = data };
        }
        public static void ForPassword(string Pass, bool Letters = true, bool UpCase = false, bool LowCase = false, bool Special = false, bool Numbers = false, int Length = 8)
        {
            if (Security.PasswordCheck.Length_checker(Pass, Length)) goto Error;
            if (LowCase && Security.PasswordCheck.LowLetters_checker(Pass)) goto Error;
            if (UpCase && Security.PasswordCheck.UpLetters_checker(Pass)) goto Error;
            if (Letters && Security.PasswordCheck.Letters_checker(Pass)) goto Error;
            if (Special && Security.PasswordCheck.Special_Characters_checker(Pass)) goto Error;
            if (Numbers && Security.PasswordCheck.Numbers_checker(Pass)) goto Error;
            goto Ok;
        Error:
            {
                throw new NotValidParameterException(typeof(string), Pass)
                {
                    Data = new
                    {
                        Letters,
                        UpCase,
                        LowCase,
                        Special,
                        Numbers,
                        Length,
                    }
                };
            }
        Ok:
            {

            }
        }
        public static void ForGuid(string data, out Guid Id)
        {
            if (!Guid.TryParse(data, out Id)) throw new NotValidParameterException(typeof(Guid), data);
        }
    }
}
