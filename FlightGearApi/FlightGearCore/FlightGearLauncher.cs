using FlightGearApi.Enums;
using FlightGearApi.records;
using System.Diagnostics;

namespace FlightGearApi.FlightGearCore;

/// <summary>
/// Управляет запуском/отключением Flight Gear с хранимыми параметрами запуска.
/// Хранит параметры ввода/вывода. 
/// </summary>
public class FlightGearLauncher
{
    private Process flightGearProcess;
    /// <summary>
    /// Конфигурация из appsettings.json
    /// </summary>
    private IConfiguration _configuration { get; }
    
    /// <summary>
    /// Путь к корневой папке FlightGear
    /// </summary>
    public string FlightGearPath { get; }
    
    public string FlightGearExectuablePath { get; }
    
    public bool IsRunning { get; private set; }
    public IoManager IoManager { get; }

    /// <summary>
    /// Параметры запуска для --key=value, но если value == null, то --key
    /// </summary>
    public Dictionary<string, object?> LaunchSettings { get; } = new Dictionary<string, object?>();

    /// <summary>
    /// Список параметров для открытия соединений по UDP.
    /// Ключ = Port, значение = GenericConnectionInfo
    /// </summary>
    /// <remarks>Решил отделить, но можно поменять вариант реализации</remarks>
    public Dictionary<int, GenericConnectionInfo> GenericConnectionsList = new ();
    
    public FlightGearLauncher(IoManager ioManager, IConfiguration configuration)
    {
        IoManager = ioManager;
        _configuration = configuration;
        FlightGearExectuablePath = Path.Combine(configuration.GetSection("FlightGear:Path").Value,
                                   configuration.GetSection("FlightGear:BinarySubPath").Value,
                                   configuration.GetSection("FlightGear:ExecutableFileName").Value + ".exe");
        // Для тестирования
        LaunchSettings["aircraft"] = "c172p";
        LaunchSettings["disable-clouds"] = null;
        LaunchSettings["disable-sound"] = null;
        LaunchSettings["in-air"] = null;
        LaunchSettings["enable-freeze"] = null;
        LaunchSettings["airport"] = "KSFO";
        LaunchSettings["altitude"] = 7224;
    }

    public void LaunchSimulation()
    {
        flightGearProcess = new Process();
        
        // Перемнная окружения, необходимая, для запуска FlightGear
        flightGearProcess.StartInfo.EnvironmentVariables["FG_ROOT"] = Path.Combine(
            $"{_configuration.GetSection("FlightGear:Path").Value}", "data");
        
        flightGearProcess.StartInfo.FileName = FlightGearExectuablePath; // путь к исполняемому файлу FlightGear
        flightGearProcess.Start(); // запуск FlightGear без параметров
        
        // TODO: Добавить проверку, чтобы нельзя было запустить сразу несколько симуляций, потому что у нас только 1 объект процесса
        // TODO: 2. Запуск с параметрами
        // TODO: 3. Запуск с параметрами GenericConnectionInfo
    }

    public void ExitSimulation()
    {
        if (flightGearProcess != null && !flightGearProcess.HasExited)
        {
            flightGearProcess.Kill(); // завершение процесса
            flightGearProcess.Dispose(); // очистка ресурсов
        }
    }

    public string GenerateParameterGenericConnection(GenericConnectionInfo connectionInfo)
    {
        // TODO: генерация параметра открытия соединения по UDP
        return "fgfs";
    }

    public void AddLaunchParameter(string name, string? value = null)
    {
        // TODO: Добавление параметра запуска
    }
    
    public void RemoveLaunchParameter(string name)
    {
        // TODO: Удаления параметра запуска
    }
    
    public void ChangeLaunchParameter(string name, string newValue)
    {
        // TODO: Изменение значения параметра запуска
    }

    public void AddGenericConnection(IoType ioType, int port, int refreshesPerSecond, string protocolFileName, string address="127.0.0.1")
    {
        // TODO: Добавление UDP соединения
    }
    
}