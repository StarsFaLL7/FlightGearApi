namespace FlightGearApi.Domain.Logging;

public static class StaticLogger
{
    public static string LogFile { get; private set; }
    public static bool LogStarted;
    
    public static void StartNewLog()
    {
        LogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @$"logs\log-{DateTime.Now.ToString("dd/MM/yyyy_HH-mm")}.log");
        if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs")))
        {
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs"));
        }
        LogStarted = true;
    }
    
    public static async Task LogAsync(LogLevel logLevel, string message)
    {
        if (!LogStarted)
        {
            throw new InvalidOperationException($"Tried to log info, but Logging didn't start. LogLevel: {logLevel}, Message: {message}");
        }

        var str = $"[{DateTime.Now.ToString("HH:mm:ss:fff")}] {logLevel.ToString()}: {message}\n";
        using (var writer = new StreamWriter(LogFile, true))
        {
            await writer.WriteAsync(str);
        }
        Console.Write(str);
    }
}