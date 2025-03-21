using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MrX.Web.Logger;

public class SecurityLogger
{
    private readonly object _l = new();
    private readonly Action<string, object, bool>? _actionLog;
    private readonly ILogger _logger;
    private readonly bool _toConsole;
    private readonly bool _toFile;

    public SecurityLogger(ILoggerFactory logger, bool toFile = false, bool toConsole = true,
        Action<string, object, bool>? costomLog = null)
    {
        _actionLog = costomLog;
        this._toFile = toFile;
        this._toConsole = toConsole;
        this._logger = logger.CreateLogger(typeof(SecurityLogger));
        if (toFile)
            if (!Directory.Exists("Log"))
                _ = Directory.CreateDirectory("Log");
        this._logger.LogInformation("SecurityLogger Created");
    }

    public Task Log(string id, object message, bool asJson = true)
    {
        var text = (asJson ? JsonConvert.SerializeObject(message) : message).ToString();
        if (_toFile)
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
                _logger.LogError(ex.Message);
            }

        if (_toConsole) _logger.Log(LogLevel.Information, $"{DateTime.Now}:::{id}=>{text} \n");
        _actionLog?.Invoke(id, message, asJson);
        return Task.CompletedTask;
    }
}