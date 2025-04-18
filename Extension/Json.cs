using Newtonsoft.Json.Linq;

namespace MrX.Web.Extension;

public static class Json
{
    public static string ToJson(this object obj) => Newtonsoft.Json.JsonConvert.SerializeObject(obj);
    public static T? FromJson<T>(this string json) => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
    public static object? FromJson(this string json) => Newtonsoft.Json.JsonConvert.DeserializeObject(json);
    public static JObject ToJObject(this string json) => Newtonsoft.Json.Linq.JObject.Parse(json);
}