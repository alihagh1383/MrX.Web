namespace MrX.Web.Extension;

public static class Json
{
    public static string ToJson(this object obj) => Newtonsoft.Json.JsonConvert.SerializeObject(obj);
}