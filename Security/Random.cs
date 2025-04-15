namespace MrX.Web.Security;

public class Random
{
    public static string String(
        int len,
        bool lowerCase = false,
        bool uppercase = false,
        bool number = true,
        int? seed = 0
    )
    {
        var random = new System.Random(seed ?? System.Random.Shared.Next());
        var list = new List<char>();
        if (lowerCase)
            for (var c = 'a'; c <= 'z'; ++c)
                list.Add(c);
        if (uppercase)
            for (var c = 'A'; c <= 'Z'; ++c)
                list.Add(c);
        if (number)
            for (short c = 0; c <= 9; ++c)
                list.Add(c.ToString()[0]);

        var o = random.GetItems(list.ToArray(), len);
        return string.Join("", o);
    }
}