namespace MrX.Web.Security;

public class Random
{
    public static string String(
        int len,
        bool upercase = false,
        bool number = true
    )
    {
        var list = new List<char>();
        for (var c = 'a'; c <= 'z'; ++c)
            list.Add(c);
        if (upercase)
            for (var c = 'A'; c <= 'Z'; ++c)
                list.Add(c);
        if (number)
            for (short c = 0; c <= 9; ++c)
                list.Add(c.ToString()[0]);
        var o = System.Random.Shared.GetItems(list.ToArray(), len);
        return string.Join("", o);
    }
}