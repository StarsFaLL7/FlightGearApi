using Application.Enums;
using Application.Interfaces;
using Application.Interfaces.Connection;
using Application.Interfaces.Entities;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Master;

/// <inheritdoc />
internal class UserSimulationMasterService : IUserSimulationMasterService
{
    private readonly IFlightGearLauncher _flightGearLauncher;
    private readonly IXmlFileManager _xmlFileManager;
    private readonly IConnectionManager _connectionManager;
    private readonly IFlightExportedParametersReader _flightExportedParametersReader;
    private readonly IServiceProvider _serviceProvider;

    private FlightStatus _currentStatus = FlightStatus.NotRunning;
    private FlightSession? _runningSession;
    public UserSimulationMasterService(IFlightGearLauncher flightGearLauncher,
        IXmlFileManager xmlFileManager, IConnectionManager connectionManager, IFlightExportedParametersReader flightExportedParametersReader,
        IServiceProvider serviceProvider)
    {
        _flightGearLauncher = flightGearLauncher;
        _xmlFileManager = xmlFileManager;
        _connectionManager = connectionManager;
        _flightExportedParametersReader = flightExportedParametersReader;
        _serviceProvider = serviceProvider;
    }
    
    public async Task StartSimulationWithFlightPlanAsync(Guid flightPlanId, FlightSession flightSession)
    {
        _currentStatus = FlightStatus.Launching;
        try
        {
            var flightPlanService = _serviceProvider.GetRequiredService<IFlightPlanService>();
            var flightPlan = await flightPlanService.GetAggregatedFlightPlanAsync(flightPlanId);
            await _xmlFileManager.CreateOrUpdateRouteManagerXmlFileAsync(flightPlan);
            await _xmlFileManager.CreateOrUpdateExportXmlFileAsync();
            await _flightGearLauncher.InitializeWithFlightPlanAsync(flightPlan);
            await _flightGearLauncher.TryLaunchSimulationAsync(flightSession);
            _currentStatus = FlightStatus.Running;
            _runningSession = flightSession;
            UpdateCycle();
        }
        catch (Exception e)
        {
            _currentStatus = FlightStatus.Exited;
            throw;
        }
    }

    public async Task ExitSimulationAsync()
    {
        if (_currentStatus != FlightStatus.Running)
        {
            throw new Exception("Simulation is not running.");
        }
        await _flightGearLauncher.CloseFlightGearAsync();
        ResetStatus();
    }

    public async Task ExitSimulationWithPropertySaveAsync()
    {
        if (_currentStatus != FlightStatus.Running || _runningSession == null)
        {
            throw new Exception("Simulation is not running.");
        }
        _runningSession.DurationSec = (int)(DateTime.Now.ToUniversalTime() - _runningSession.DateTimeStart).TotalSeconds;
        await _flightGearLauncher.CloseFlightGearAsync();
        await Task.Delay(500);
        var propertiesShots = await _flightExportedParametersReader.GetExportedPropertiesAsync(_runningSession.Id);
        var propertiesShotRepository = _serviceProvider.GetRequiredService<IFlightPropertiesShotRepository>();
        await propertiesShotRepository.SaveRangeAsync(propertiesShots);
        var sessionService = _serviceProvider.GetRequiredService<ISessionService>();
        await sessionService.SaveSessionAsync(_runningSession);
        ResetStatus();
    }

    private void ResetStatus()
    {
        _currentStatus = FlightStatus.Finished;
        _runningSession = null;
    }

    public async Task<FlightStatus> GetSimulationStatus()
    {
        return _currentStatus;
    }

    public async Task<int> GetLastReachedRoutePointOrderAsync()
    {
        var currentWp = await _connectionManager.GetPropertyDoubleValueAsync("autopilot/route-manager/current-wp");
        return (int)(currentWp - 1);
    }

    public async Task<int> GetRoutePercentCompletionAsync()
    {
        var totalDistance = await _connectionManager.GetPropertyDoubleValueAsync("autopilot/route-manager/total-distance");
        var leftDistance = await _connectionManager.GetPropertyDoubleValueAsync("autopilot/route-manager/wp-last/dist");
        return (int)((totalDistance - leftDistance) * 100 / totalDistance);
    }

    public async Task<FlightPropertiesShot> GetCurrentFlightValuesAsync()
    {
        var values = await _connectionManager.GetCurrentValuesAsync();
        return values;
    }
    
    /// <summary>
    /// Запуск цикла обновления для своевременного выхода из симуляции. Цикл крутится всё время, пока идёт симуляция.
    /// </summary>
    private async Task UpdateCycle()
    {
        while (_currentStatus == FlightStatus.Running)
        {
            var currentWp = await _connectionManager.GetPropertyDoubleValueAsync("autopilot/route-manager/current-wp");
            if ((int)currentWp == -1)
            {
                await ExitSimulationWithPropertySaveAsync();
                break;
            }
            await Task.Delay(500);
        }
    }
}