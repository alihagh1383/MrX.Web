namespace MrX.Web.Security;

public static class Random
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="len"></param>
    /// <param name="lowerCase"></param>
    /// <param name="uppercase"></param>
    /// <param name="number"></param>
    /// <param name="seed">set null for random seed</param>
    /// <returns></returns>
    public static string String(
        int len,
        bool lowerCase = false,
        bool uppercase = false,
        bool number = true,
        int? seed = null,
        bool whiteSpase = false,
        char[]? lowerCaseSet = null
    )
    {
        var list = new List<char>();
        if (uppercase)
            for (var c = 'A'; c <= 'Z'; ++c) list.Add(c);
        if (number)
            for (short c = 0; c <= 9; ++c) list.Add(c.ToString()[0]);
        if (whiteSpase)
            list.Add(' ');
        if (lowerCase)
            if (lowerCaseSet is null)
                for (var c = 'a'; c <= 'z'; ++c) list.Add(c);
            else if (lowerCaseSet is { })
                list.AddRange(lowerCaseSet);
        var o = ((seed != null) ? new System.Random((int)seed!) : System.Random.Shared).GetItems(list.ToArray(), len);
        return string.Join("", o);
    }
}