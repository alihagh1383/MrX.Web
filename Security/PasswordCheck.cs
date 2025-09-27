using System.Text.RegularExpressions;

namespace MrX.Web.Security;

public static partial class PasswordCheck
{
    public static bool Length_checker(string password, int minLength = 8, int maxLength = int.MaxValue)
    {
        return !string.IsNullOrEmpty(password) && password.Length >= minLength && password.Length <= maxLength;
    }
    public static bool UpLetters_checker(string password)
    {
        return !string.IsNullOrEmpty(password) && Upper().IsMatch(password);
    }
    public static bool LowLetters_checker(string password)
    {
        return !string.IsNullOrEmpty(password) && Lower().IsMatch(password);
    }
    public static bool Letters_checker(string password)
    {
        return !string.IsNullOrEmpty(password) && Letter().IsMatch(password);
    }
    public static bool Numbers_checker(string password)
    {
        return !string.IsNullOrEmpty(password) && Number().IsMatch(password);
    }
    public static bool Special_Characters_checker(string password)
    {
        return !string.IsNullOrEmpty(password) && Special().IsMatch(password);
    }

    [GeneratedRegex("[A-Z]")]
    public static partial Regex Upper();
    [GeneratedRegex("[a-z]")]
    public static partial Regex Lower();
    [GeneratedRegex("[A-Za-z]")]
    public static partial Regex Letter();
    [GeneratedRegex("[0-9]")]
    public static partial Regex Number();
    [GeneratedRegex("[!@#$%^&*()]")]
    public static partial Regex Special();
}
