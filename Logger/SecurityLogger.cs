using Newtonsoft.Json;

namespace MrX.Web.Logger;

public class SecurityLogger
{
    public SecurityLogger()
    {
        if (!Directory.Exists("Log")) _ = Directory.CreateDirectory("Log");
    }

    private readonly object _l = new();

    public Task Log(string id, object message, bool asJson = true)
    {
        TRY:
        try
        {
            var file = Path.Combine("Log", DateOnly.FromDateTime(DateTime.Now).ToLongDateString() + ".Log");
            var text = ((asJson) ? JsonConvert.SerializeObject(message) : message).ToString();
            if (string.IsNullOrWhiteSpace(text)) Console.WriteLine("null");
            lock (_l)
            {
                File.AppendAllText(file, $"{DateTime.Now}:::{id}=>{text} \n");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return Task.CompletedTask;
    }
}