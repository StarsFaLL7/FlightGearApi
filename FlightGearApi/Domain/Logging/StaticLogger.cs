namespace FlightGearApi.Domain.Logging;

public static class StaticLogger
{
    private static string? _logFile;
    private static bool _logStarted;
    
    public static void StartNewLog()
    {
        _logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @$"logs\log-{DateTime.Now.ToString("dd/MM/yyyy_HH-mm")}.log");
        if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs")))
        {
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs"));
        }
        _logStarted = true;
    }
    
    public static async Task LogAsync(LogLevel logLevel, string message)
    {
        if (!_logStarted || _logFile is null)
        {
            throw new InvalidOperationException($"Tried to async log info, but Logging didn't start. LogLevel: {logLevel}, Message: {message}");
        }

        var str = $"[{DateTime.Now.ToString("HH:mm:ss:fff")}] [{logLevel.ToString()}] {message}\n";
        await using (var writer = new StreamWriter(_logFile, true))
        {
            await writer.WriteAsync(str);
        }
        Console.Write(str);
    }

    public static void Log(LogLevel logLevel, string message)
    {
        if (!_logStarted || _logFile is null)
        {
            throw new InvalidOperationException($"Tried to log info, but Logging didn't start. LogLevel: {logLevel}, Message: {message}");
        }

        var str = $"[{DateTime.Now.ToString("HH:mm:ss:fff")}] [{logLevel.ToString()}]: {message}\n";
        using (var writer = new StreamWriter(_logFile, true))
        {
            writer.Write(str);
        }
        Console.Write(str);
    }
}