using System.Text.RegularExpressions;

namespace MrX.Web.Security;

public static class PasswordCheck
{
    public static bool Length_checker(string password, int minLength = 8, int maxLength = int.MaxValue)
    {
        if (string.IsNullOrEmpty(password) || password.Length < minLength || password.Length > maxLength) return false;
        return true;
    }
    public static bool UpLetters_checker(string password)
    {
        if (string.IsNullOrEmpty(password) || !Regex.IsMatch(password, "[A-Z]")) return false;
        return true;
    }
    public static bool LowLetters_checker(string password)
    {
        if (string.IsNullOrEmpty(password) || !Regex.IsMatch(password, "[a-z]")) return false;
        return true;
    }
    public static bool Letters_checker(string password)
    {
        if (string.IsNullOrEmpty(password) || !Regex.IsMatch(password, "[A-Za-z]")) return false;
        return true;
    }
    public static bool Numbers_checker(string password)
    {
        if (string.IsNullOrEmpty(password) || !Regex.IsMatch(password, "[0-9]")) return false;
        return true;
    }
    public static bool Special_Characters_checker(string password)
    {
        if (string.IsNullOrEmpty(password) || !Regex.IsMatch(password, "[!@#$%^&*()]")) return false;
        return true;
    }
}