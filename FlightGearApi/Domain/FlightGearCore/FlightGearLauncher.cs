﻿using System.Diagnostics;
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
        
        Reset();
    }

    public async Task<bool> TryLaunchSimulation(string sessionName)
    {
        if (_flightGearProcess != null && _flightGearProcess.HasExited == false)
        {
            return false;
        }

        try
        {
            IoManager.SaveXmlFiles();
            Listener.Reset();
            
            _flightGearProcess = new Process();
        
            _flightGearProcess.StartInfo.EnvironmentVariables["FG_ROOT"] = Path.Combine(
                $"{Configuration.GetSection("FlightGear:Path").Value}", "data");
        
            _flightGearProcess.StartInfo.FileName = FlightGearExecutablePath; // путь к исполняемому файлу FlightGear

            _flightGearProcess.StartInfo.Arguments += GenerateAllParametersString();
        
        
            _flightGearProcess.StartInfo.RedirectStandardOutput = true;
            _flightGearProcess.StartInfo.RedirectStandardError = true;
            _flightGearProcess.StartInfo.UseShellExecute = false;
        
        
            _flightGearProcess.Start();
        
            _flightGearProcess.BeginOutputReadLine();
            _flightGearProcess.BeginErrorReadLine();
            _flightGearProcess.OutputDataReceived += (sender, e) =>
            {
                Console.WriteLine(e.Data);
            
                if (e.Data != null && e.Data.Contains("Run Count"))
                {
                    IsRunning = true;
                
                    Console.WriteLine("FlightGear initialization complete.");
                }
            };
        
            // Ожидание инициализации
            var timeout = 50000;
            var time = 0;
            while (!IsRunning)
            {
                await Task.Delay(100);
                time += 100;
                if (time > timeout)
                {
                    _flightGearProcess = null;
                    Listener.TryStopListen();
                    IsRunning = false;
                    throw new TimeoutException("Не удалось запустить Flight Gear.");
                }
            }

            RunningSessionName = sessionName;
            Listener.TryStartListen(sessionName);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public string GenerateAllParametersString()
    {
        var resultString = "";
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
        
        resultString += IoManager.GenerateParametersGenericConnection();

        return resultString;
    }

    public bool TryExitSimulation()
    {
        try
        {
            _flightGearProcess.Kill(); // завершение процесса
            _flightGearProcess.Dispose(); // очистка ресурсов
            _flightGearProcess = null;
            Listener.TryStopListen();
            IsRunning = false;
            return true;
        }
        catch (NullReferenceException e)
        {
            return false;
        }
    }

    private void Reset()
    {
        LaunchArguments.Clear();
        TryExitSimulation();
        RunningSessionName = "empty";
        LaunchArguments["aircraft"] = "c172p";
        LaunchArguments["disable-clouds"] = null;
        LaunchArguments["disable-sound"] = null;
        LaunchArguments["in-air"] = null;
        LaunchArguments["airport"] = "KSFO";
        LaunchArguments["altitude"] = "7224";
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