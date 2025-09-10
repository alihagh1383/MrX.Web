using System.Text.RegularExpressions;

namespace MrX.Web.Security;

public static partial class PasswordCheck
{
    public static bool Length_checker(string password, int minLength = 8, int maxLength = int.MaxValue)
    {
        if (string.IsNullOrEmpty(password) || password.Length < minLength || password.Length > maxLength) return false;
        return true;
    }
    public static bool UpLetters_checker(string password)
    {
        if (string.IsNullOrEmpty(password) || !Upper().IsMatch(password)) return false;
        return true;
    }
    public static bool LowLetters_checker(string password)
    {
        if (string.IsNullOrEmpty(password) || !Lower().IsMatch(password)) return false;
        return true;
    }
    public static bool Letters_checker(string password)
    {
        if (string.IsNullOrEmpty(password) || !Letter().IsMatch(password)) return false;
        return true;
    }
    public static bool Numbers_checker(string password)
    {
        if (string.IsNullOrEmpty(password) || !Number().IsMatch(password)) return false;
        return true;
    }
    public static bool Special_Characters_checker(string password)
    {
        if (string.IsNullOrEmpty(password) || !Special().IsMatch(password)) return false;
        return true;
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
