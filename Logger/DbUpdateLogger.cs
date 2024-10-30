using System.Security.Cryptography;

namespace MrX.Web.Logger;

public class DbUpdateLogger
{
    public DbUpdateLogger(string tableName, string entityId, object entity)
    {
        if (!Directory.Exists("Log"))
            _ = Directory.CreateDirectory("Log");
        if (!Directory.Exists(Path.Combine("Log", tableName)))
            _ = Directory.CreateDirectory(Path.Combine("Log", tableName));
        File.AppendAllText(
            Path.Combine("Log", tableName, entityId),
            Newtonsoft.Json.JsonConvert.SerializeObject(entity));
    }
}