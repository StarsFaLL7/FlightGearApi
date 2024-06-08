using System.Diagnostics;
using System.Text;
using Application.Interfaces;
using Application.Interfaces.Connection;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Application.Services;

internal class FlightGearLauncher : IFlightGearLauncher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnectionManager _connectionManager;

    private readonly int _telnetPort;
    private readonly int _httpPort;
    private readonly string _defaultLaunchArguments;
    private readonly string _binPath;
    private readonly string _exportXmlFileName;
    private readonly string _exportTextFilePath;
    private readonly string _routeXmlFilePath;
    private readonly string _fgDataPath;
    private readonly string _exeFileName;

    private bool _isInitialized;
    private bool _isStartFromAirport;
    private Airport? _startAirport;
    private AirportRunway? _startRunway;
    private RoutePoint? _startRoutePoint;
    private double _startHeading;
    
    private Process? _flightGearProcess;

    private bool _isRunning;

    public FlightGearLauncher(IServiceProvider serviceProvider, IConfiguration configuration, IConnectionManager connectionManager)
    {
        _serviceProvider = serviceProvider;
        _connectionManager = connectionManager;
        var fgSection = configuration.GetSection("FlightGearSettings");
        _defaultLaunchArguments = fgSection.GetValue<string>("DefaultLaunchArgs");
        
        var foldersSection = fgSection.GetSection("Folders");
        var mainFolderPath = foldersSection.GetValue<string>("MainFolder");
        _binPath = Path.Combine(mainFolderPath, foldersSection.GetValue<string>("BinarySubPath"));
        _exeFileName = foldersSection.GetValue<string>("ExecutableFileName");
        _fgDataPath = Path.Combine(mainFolderPath, "data");
        
        var exportSection = fgSection.GetSection("Export");
        _exportXmlFileName = exportSection.GetValue<string>("XmlExportFilename");
        _exportTextFilePath = Path.Combine(mainFolderPath, exportSection.GetValue<string>("ExportPropertiesFileName"));
        
        var routeSection = fgSection.GetSection("RouteManager");
        _routeXmlFilePath = Path.Combine(routeSection.GetValue<string>("RoutePlanPath"), 
            routeSection.GetValue<string>("RoutePlanFileName"));

        var connSection = fgSection.GetSection("Connections");
        _telnetPort = connSection.GetValue<int>("TelnetPort");
        _httpPort = connSection.GetValue<int>("HttpPort");
    }
    
    public async Task InitializeWithFlightPlanAsync(FlightPlan flightPlan)
    {
        _isStartFromAirport = flightPlan.DepartureRunwayId is not null;
        var scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var airportRepository = scope.ServiceProvider.GetRequiredService<IAirportRepository>();
        var flightPlanRepository = scope.ServiceProvider.GetRequiredService<IFlightPlanRepository>();
        var airportRunwayRepository = scope.ServiceProvider.GetRequiredService<IAirportRunwayRepository>();

        if (_isStartFromAirport)
        {
            _startRunway = await airportRunwayRepository.GetByIdAsync(flightPlan.DepartureRunwayId.Value);
            _startAirport = await airportRepository.GetByIdAsync(_startRunway.AirportId);
        }
        else
        {
            var planFull = await flightPlanRepository.GetAggregateByIdAsync(flightPlan.Id);
            var first2Points = planFull.RoutePoints.OrderBy(p => p.Order).Take(2).ToArray();
            _startRoutePoint = first2Points[0];
            _startHeading = GeographyHelper.GetDirectionDeg(_startRoutePoint.Latitude, _startRoutePoint.Longitude,
                first2Points[1].Latitude, first2Points[1].Longitude);
        }

        _isInitialized = true;
    }

    public async Task TryLaunchSimulationAsync(FlightSession flightSession)
    {
        if (!_isInitialized)
        {
            throw new Exception("First you need to initialize FlightGearLauncher with flightPlan.");
        }
        
        if (_flightGearProcess != null)
        {
            throw new Exception("Simulation hasn't exited.");
        }

        _isRunning = false;
        try
        {
            Console.WriteLine("Trying to launch Flight Gear...");

            var arguments = GetLaunchString(flightSession.PropertiesReadsPerSec);
            
            _flightGearProcess = new Process();
        
            _flightGearProcess.StartInfo.FileName = Path.Combine(_binPath, _exeFileName + ".exe"); // путь к исполняемому файлу FlightGear
            _flightGearProcess.StartInfo.Arguments += arguments;
            
            _flightGearProcess.Start();

            var tries = 0;
            while (!_isRunning)
            {
                try
                {
                    var launched = await _connectionManager.GetPropertyStringValueAsync("/sim/fdm-initialized");
                    if (launched == "true")
                    {
                        Console.WriteLine("FlightGear launched successfully!");
                        await SendInitialParameters();
                        _isRunning = true;
                        return;
                    }

                    tries++;
                    if (tries > 100)
                    {
                        throw new Exception($"Simulation didn't start after {tries} tries.");
                    }
                }
                catch (Exception e)
                {
                    // ignored
                }
            }

            throw new Exception("Launch failed. Unknown reason.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while launching Flight Gear: {e}");
            throw;
        }
    }

    private async Task SendInitialParameters()
    {
        await ChangePause(true);
        await _connectionManager.SetPropertyAsync("autopilot/locks/altitude", "altitude-hold");
        await _connectionManager.SetPropertyAsync("autopilot/locks/speed", "speed-with-throttle");
        await _connectionManager.SetPropertyAsync("autopilot/locks/heading", "true-heading-hold");
        await _connectionManager.SetPropertyAsync("autopilot/settings/target-speed-kt", 600);
        
        await _connectionManager.SetPropertyAsync("autopilot/route-manager/input", "@activate");
        
        await _connectionManager.SetPropertyAsync("canopy/position-norm", 0);
        await _connectionManager.SetPropertyAsync("sim/current-view/view-number", 1);
        await _connectionManager.SetPropertyAsync("sim/current-view/field-of-view", 90);
        await _connectionManager.SetPropertyAsync("sim/current-view/x-offset-m", 0);
        await _connectionManager.SetPropertyAsync("sim/current-view/y-offset-m", 10);

        await ChangePause(false);
    }
    
    private async Task ChangePause(bool pause)
    {
        await _connectionManager.SetPropertyAsync("/sim/freeze/master", pause);
        await _connectionManager.SetPropertyAsync("/sim/freeze/clock", pause);
    }
    
    public async Task CloseFlightGearAsync()
    {
        if (!_isRunning)
        {
            throw new Exception("Simulation is not running.");
        }

        _flightGearProcess?.Kill();
        _flightGearProcess?.Dispose();
        _flightGearProcess = null;
        _isRunning = false;
        _isInitialized = false;
    }

    public string GetLaunchString(int propertiesReadsPerSecond)
    {
        var sb = new StringBuilder();
        sb.Append($" --fg-root=\"{_fgDataPath}\"");
        sb.Append($" --generic=file,out,{propertiesReadsPerSecond},\"{_exportTextFilePath}\",\"{_exportXmlFileName}\"");
        sb.Append($" --httpd={_httpPort}");
        sb.Append($" --telnet=socket,bi,60,localhost,{_telnetPort},tcp");
        sb.Append($" --flight-plan=\"{_routeXmlFilePath}\"");
        sb.Append(" --allow-nasal-from-sockets");
        if (!_isStartFromAirport)
        {
            var altitude = _startRoutePoint.Altitude * 3.28084; // переводим метры в футы
            sb.Append(" --in-air");
            sb.Append($" --altitude={altitude}");
            sb.Append($" --lat={_startRoutePoint.Latitude}");
            sb.Append($" --lon={_startRoutePoint.Longitude}");
            sb.Append($" --heading={_startHeading}");
            sb.Append(" --vc=300");
        }
        else
        {
            sb.Append($" --airport={_startAirport.Code}");
            sb.Append($" --runway={_startRunway.Title}");
        }

        if (_defaultLaunchArguments.Length > 0)
        {
            if (_defaultLaunchArguments[0] != ' ')
            {
                sb.Append(' ');
            }
            sb.Append(_defaultLaunchArguments);
        }

        return sb.ToString();
    }

    public string GetExportedTextDataFilePath()
    {
        return _exportTextFilePath;
    }
}