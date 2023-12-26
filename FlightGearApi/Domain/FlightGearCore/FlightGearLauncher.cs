using System.Diagnostics;
using FlightGearApi.Domain.Enums;
using FlightGearApi.Domain.Logging;
using FlightGearApi.Domain.Records;
using FlightGearApi.Infrastructure.Interfaces;
using FlightGearApi.Infrastructure.ModelsDal;

namespace FlightGearApi.Domain.FlightGearCore;

/// <summary>
/// Управляет запуском/отключением Flight Gear с хранимыми параметрами запуска.
/// Хранит параметры ввода/вывода. 
/// </summary>
public class FlightGearLauncher
{
    private Process? _flightGearProcess;
    private Dictionary<string, string?> LaunchArguments { get; set; } = new ();
    private IConfiguration Configuration { get; }
    private ConnectionListener Listener { get; }
    private IoManager IoManager { get; }
    private FlightGearManipulator Manipulator { get; }
    private string FlightGearExecutablePath { get; }

    
    public bool IsRunning { get; private set; }
    public string? RunningSessionName { get; private set; }
    public int? RunningSessionId { get; private set; } = null;
    
    public FlightGearLauncher(IoManager ioManager, IConfiguration configuration, ConnectionListener listener, 
        FlightGearManipulator manipulator)
    {
        Listener = listener;
        IoManager = ioManager;
        Manipulator = manipulator;
        Configuration = configuration;
        FlightGearExecutablePath = Path.Combine(configuration.GetSection("FlightGear:Path").Value,
                                   configuration.GetSection("FlightGear:BinarySubPath").Value,
                                   configuration.GetSection("FlightGear:ExecutableFileName").Value + ".exe");
        
        LaunchArguments["disable-ai-traffic"] = null;
        LaunchArguments["aircraft"] = "c172p";
        LaunchArguments["timeofday"] = "morning";
        LaunchArguments["disable-clouds"] = null;
        LaunchArguments["disable-sound"] = null;
        LaunchArguments["airport"] = "KSFO";
        //LaunchArguments["state"] = "auto";
        LaunchArguments["altitude"] = "3000";
        
        
    }

    public async Task<bool> TryLaunchSimulation(string sessionName, double refreshes, IPostgresDatabase database)
    {
        if (_flightGearProcess != null && _flightGearProcess.HasExited == false)
        {
            return false;
        }
        try
        {
            await StaticLogger.LogAsync(LogLevel.Information, "Trying to launch Flight Gear...");
            IoManager.ConnectionRefreshesPerSecond = refreshes;
            IoManager.SaveXmlFile();
            Listener.ClearResults();
            
            _flightGearProcess = new Process();
        
            _flightGearProcess.StartInfo.EnvironmentVariables["FG_ROOT"] = Path.Combine(
                $"{Configuration.GetSection("FlightGear:Path").Value}", "data");
            _flightGearProcess.StartInfo.FileName = FlightGearExecutablePath; // путь к исполняемому файлу FlightGear
            _flightGearProcess.StartInfo.Arguments += GenerateLaunchArguments();
            
            _flightGearProcess.StartInfo.RedirectStandardOutput = true;
            _flightGearProcess.StartInfo.RedirectStandardError = true;
            _flightGearProcess.StartInfo.UseShellExecute = false;
            
            _flightGearProcess.Start();
        
            _flightGearProcess.BeginOutputReadLine();
            _flightGearProcess.BeginErrorReadLine();
            _flightGearProcess.OutputDataReceived += (sender, e) =>
            {
                Console.WriteLine(e.Data);
            
                if (e.Data != null && e.Data.Contains("Now checking"))
                {
                    IsRunning = true;
                    Listener.IsFlightGearRunning = true;
                    Console.WriteLine("FlightGear initialization complete.");
                }
            };
        
            // Ожидание инициализации
            var timeout = 120000;
            var time = 0;
            while (!IsRunning)
            {
                await Task.Delay(100);
                time += 100;
                if (time > timeout)
                {
                    _flightGearProcess?.Kill();
                    _flightGearProcess = null;
                    IsRunning = false;
                    Listener.IsFlightGearRunning = false;
                    throw new TimeoutException("Не удалось запустить Flight Gear.");
                }
            }
            
            RunningSessionName = sessionName;
            await Task.Delay(30000); // Дополнительно время на ициализацию
            
            RunningSessionId = database.CreateSession(new FlightSessionDal() { Date = DateTime.Now, Title = sessionName } );
            Manipulator.SessionId = RunningSessionId;
            
            Manipulator.ShouldFlyForward = true;
            Manipulator.FlyCycle();
            
            ReloadCycle();
            return true;
        }
        catch (Exception e)
        {
            await StaticLogger.LogAsync(LogLevel.Error, $"Error while launching Flight Gear: {e}");
            throw;
        }
    }

    private async void ReloadCycle()
    {
        while (IsRunning)
        {
            if (Manipulator.AllStagesCompleted)
            {
                Exit();
            }

            await Task.Delay(1000);
        }
    }

    public string GenerateLaunchArguments()
    {
        var resultString = $"--telnet=socket,in,10,127.0.0.1,{IoManager.TelnetPort},tcp --httpd=5400";
        foreach (var launchArgument in LaunchArguments)
        {
            if (launchArgument.Value == null)
            {
                resultString += $" --{launchArgument.Key}";
            }
            else
            {
                resultString += $" --{launchArgument.Key}={launchArgument.Value}";
            }
        }
        
        resultString += IoManager.GetUdpInputConnectionString();

        var exportFileName = $"{Configuration.GetSection("FlightGear:ExportPropertiesFilePath").Value}";
        var xmlExportFileName = $"{Configuration.GetSection("FlightGear:XmlExportFilename").Value}";
        if (!File.Exists(exportFileName))
        {
            try
            {
                using (var fileStream = File.Create(exportFileName)) { }
            }
            catch (Exception e)
            {
                StaticLogger.Log(LogLevel.Error, e.Message);
                Console.WriteLine(e);
                throw;
            }
        }

        resultString += $" --generic=file,out,{IoManager.ConnectionRefreshesPerSecond},{exportFileName},{xmlExportFileName}";
        
        return resultString;
    }

    public void Exit()
    {
        _flightGearProcess?.Kill(); // завершение процесса
        _flightGearProcess?.Dispose(); // очистка ресурсов
        _flightGearProcess = null;
        IsRunning = false;
        RunningSessionId = null;
        Listener.IsFlightGearRunning = false;
    }
}