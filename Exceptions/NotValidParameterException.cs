using Microsoft.AspNetCore.Http;

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
        public static NotValidParameterException? ForPassword(string pass, bool letters = true, bool upCase = false, bool lowCase = false, bool special = false, bool numbers = false, int length = 8)
        {
            if (!Security.PasswordCheck.Length_checker(pass, length)) goto Error;
            if (lowCase && !Security.PasswordCheck.LowLetters_checker(pass)) goto Error;
            if (upCase && !Security.PasswordCheck.UpLetters_checker(pass)) goto Error;
            if (letters && !Security.PasswordCheck.Letters_checker(pass)) goto Error;
            if (special && !Security.PasswordCheck.Special_Characters_checker(pass)) goto Error;
            if (numbers && !Security.PasswordCheck.Numbers_checker(pass)) goto Error;
            goto Ok;
        Error:
            {
                return new NotValidParameterException(typeof(string), pass)
                {
                    Data = new
                    {
                        Letters = letters,
                        UpCase = upCase,
                        LowCase = lowCase,
                        Special = special,
                        Numbers = numbers,
                        Length = length,
                    }
                };
            }
        Ok:
            {
                return null;
            }
        }
        public static NotValidParameterException ForGuid(string? data, out Guid id)
        {
            if (!Guid.TryParse(data, out id)) return new NotValidParameterException(typeof(Guid), data); return null;
        }
        public static bool TryGuid(string? data, out Guid id, out NotValidParameterException? exception)
        {
            exception = null;
            if (Guid.TryParse(data, out id)) return true;
            exception = new NotValidParameterException(typeof(Guid), data);
            return false;
        }
    }
}
