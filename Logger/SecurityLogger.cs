using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MrX.Web.Logger;

public class SecurityLogger
{
    bool ToFile;
    bool ToConsole; Microsoft.Extensions.Logging.ILogger logger;
    public SecurityLogger(Microsoft.Extensions.Logging.ILoggerFactory logger, bool ToFile = false, bool ToConsole = true)
    {
        this.ToFile = ToFile;
        this.ToConsole = ToConsole;
        this.logger = logger.CreateLogger(typeof(SecurityLogger));
        if (ToFile)
            if (!Directory.Exists("Log")) _ = Directory.CreateDirectory("Log");
    }

    private readonly object _l = new();

    public Task Log(string id, object message, bool asJson = true)
    {
        var text = ((asJson) ? JsonConvert.SerializeObject(message) : message).ToString();
        if (this.ToFile)
        {
            try
            {
                var file = Path.Combine("Log", DateOnly.FromDateTime(DateTime.Now).ToLongDateString() + ".Log");
                lock (_l) File.AppendAllText(file, $"{DateTime.Now}:::{id}=>{text} \n");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
        if (ToConsole) logger.Log(LogLevel.Information,$"{DateTime.Now}:::{id}=>{text} \n");
        return Task.CompletedTask;
    }
}