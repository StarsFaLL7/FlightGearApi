using System.Diagnostics;
using FlightGearApi.Domain.Enums;
using FlightGearApi.Domain.Records;

namespace FlightGearApi.Domain.FlightGearCore;

/// <summary>
/// Управляет запуском/отключением Flight Gear с хранимыми параметрами запуска.
/// Хранит параметры ввода/вывода. 
/// </summary>
public class FlightGearLauncher
{
    private Process? _flightGearProcess;

    public bool IsRunning { get; private set; }

    public string RunningSessionName { get; private set; }
    
    private IConfiguration Configuration { get; }
    private ConnectionListener Listener { get; }
    
    private string FlightGearExecutablePath { get; }
    
    public IoManager IoManager { get; }

    /// <summary>
    /// Параметры запуска для --key=value, но если value == null, то --key
    /// </summary>
    private Dictionary<string, string?> LaunchArguments { get; set; } = new ();
    
    public FlightGearLauncher(IoManager ioManager, IConfiguration configuration, ConnectionListener listener)
    {
        Listener = listener;
        IoManager = ioManager;
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
        //LaunchArguments["vc"] = "150";
        
    }

    public async Task<bool> TryLaunchSimulation(string sessionName, double refreshes)
    {
        if (_flightGearProcess != null && _flightGearProcess.HasExited == false)
        {
            return false;
        }
        try
        {
            IoManager.SetRefreshesPerSecond(refreshes);
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
            Listener.StartListenForClient(sessionName);
            await Task.Delay(20000); // Допольнительно время на ициализацию
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
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
        
        resultString += IoManager.ConvertGenericConnectionToArgument(IoManager.GetInputConnectionInfo());

        return resultString;
    }

    public void Exit()
    {
        _flightGearProcess?.Kill(); // завершение процесса
        _flightGearProcess?.Dispose(); // очистка ресурсов
        _flightGearProcess = null;
        Listener.StopListenForClient();
        IsRunning = false;
        Listener.IsFlightGearRunning = false;
    }

    public bool TryAddLaunchParameter(string name, string? value = null)
    {
        // TODO: Добавление параметра запуска
        return false;
    }
    
    public bool TryRemoveLaunchParameter(string name)
    {
        // TODO: Удаления параметра запуска
        return false;
    }
    
    public bool TryChangeLaunchParameter(string name, string newValue)
    {
        // TODO: Изменение значения параметра запуска
        return false;
    }
}