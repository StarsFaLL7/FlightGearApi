using FlightGearApi.Enums;
using FlightGearApi.records;

namespace FlightGearApi.FlightGearCore;

/// <summary>
/// Управляет запуском/отключением Flight Gear с хранимыми параметрами запуска.
/// Хранит параметры ввода/вывода. 
/// </summary>
public class FlightGearLauncher
{
    /// <summary>
    /// Путь к корневой папке FlightGear
    /// </summary>
    public string FlightGearPath { get; }
    
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
    
    public FlightGearLauncher(IoManager ioManager, string flightGearPath)
    {
        IoManager = ioManager;
        FlightGearPath = flightGearPath;
        
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
        // TODO: 1. Запуск Flight Gear
        // TODO: 2. Запуск с параметрами
        // TODO: 3. Запуск с параметрами GenericConnectionInfo
    }

    public void ExitSimulation()
    {
        // TODO: Досрочный выход из Flight Gear
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