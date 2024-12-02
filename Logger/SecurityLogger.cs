using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MrX.Web.Logger;

public class SecurityLogger
{
    private readonly object _l = new();
    private readonly Action<string, object, bool>? ActionLog;
    private readonly ILogger logger;
    private readonly bool ToConsole;
    private readonly bool ToFile;

    public SecurityLogger(ILoggerFactory logger, bool ToFile = false, bool ToConsole = true,
        Action<string, object, bool>? CostomLog = null)
    {
        ActionLog = CostomLog;
        this.ToFile = ToFile;
        this.ToConsole = ToConsole;
        this.logger = logger.CreateLogger(typeof(SecurityLogger));
        if (ToFile)
            if (!Directory.Exists("Log"))
                _ = Directory.CreateDirectory("Log");
        this.logger.LogInformation("SecurityLogger Created");
    }

    public Task Log(string id, object message, bool asJson = true)
    {
        var text = (asJson ? JsonConvert.SerializeObject(message) : message).ToString();
        if (ToFile)
            try
            {
                var file = Path.Combine("Log", DateOnly.FromDateTime(DateTime.Now).ToLongDateString() + ".Log");
                lock (_l)
                {
                    File.AppendAllText(file, $"{DateTime.Now}:::{id}=>{text} \n");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

        if (ToConsole) logger.Log(LogLevel.Information, $"{DateTime.Now}:::{id}=>{text} \n");
        ActionLog?.Invoke(id, message, asJson);
        return Task.CompletedTask;
    }
}