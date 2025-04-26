using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace MrX.Web.Extension;

public static class TempDataDictionary
{
    public static object? GetError(this ITempDataDictionary tempData) => tempData["Error"];

    public static ITempDataDictionary SetError(this ITempDataDictionary tempData, string data)
    {
        tempData["Error"] = data;
        return tempData;
    }

    public static object? GetOk(this ITempDataDictionary tempData) => tempData["Ok"];

    public static ITempDataDictionary SetOk(this ITempDataDictionary tempData, string data)
    {
        tempData["Ok"] = data;
        return tempData;
    }

    public static object? GetMessage(this ITempDataDictionary tempData) => tempData["Message"];

    public static ITempDataDictionary SetMessage(this ITempDataDictionary tempData, string data)
    {
        tempData["Message"] = data;
        return tempData;
    }
}