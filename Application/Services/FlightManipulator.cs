using Application.Enums;
using Application.Interfaces;
using Application.Interfaces.Connection;
using Application.Interfaces.Entities;
using Application.Interfaces.Repositories;
using Application.Services.Master;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services;

public class FlightManipulator : IFlightManipulator
{
    private readonly IConnectionManager _connectionManager;
    private readonly IFlightGearLauncher _flightGearLauncher;
    private readonly IServiceProvider _serviceProvider;

    public FlightManipulator(IConnectionManager connectionManager, IFlightGearLauncher flightGearLauncher, IServiceProvider serviceProvider)
    {
        _connectionManager = connectionManager;
        _flightGearLauncher = flightGearLauncher;
        _serviceProvider = serviceProvider;
    }

    public async Task InitializeAsync(FlightPlan flightPlan)
    {
        throw new NotImplementedException();
    }

    public async Task FlyCycleAsync(FlightPlan flightPlan)
    {
        while (UserSimulationMasterService.CurrentFlightStatus == FlightStatus.Running)
        {
            var currentWp = (int)(await _connectionManager.GetPropertyDoubleValueAsync("autopilot/route-manager/current-wp"));
            if (currentWp == -1)
            {
                await ExitSimulationWithPropertySaveAsync();
                break;
            }
            var currentWpId = await _connectionManager.GetPropertyStringValueAsync("autopilot/route-manager/wp/id");
            var goalSpeed = 600d;
            if (currentWpId != null && currentWpId.StartsWith("ENDF"))
            {
                goalSpeed = 300;
                var totalpointCount = (int)await _connectionManager.GetPropertyDoubleValueAsync("autopilot/route-manager/route/num");
                var lastPointId = await _connectionManager.GetPropertyStringValueAsync($"autopilot/route-manager/route/wp[{totalpointCount-1}]/id");
                if (currentWpId == lastPointId)
                {
                    goalSpeed = 20;
                }
            }
            else
            {
                var currentHeading = await _connectionManager.GetPropertyDoubleValueAsync("orientation/true-heading-deg");
            
                if (currentHeading > 180)
                {
                    currentHeading = 360 - currentHeading;
                }
                var goalHeading = await _connectionManager.GetPropertyDoubleValueAsync("autopilot/route-manager/wp/true-bearing-deg");
                if (goalHeading > 180)
                {
                    goalHeading = 360 - goalHeading;
                }
                goalSpeed = Math.Max(200, 600 - Math.Abs(currentHeading - goalHeading) / 90 * 600);
            }
            
            await _connectionManager.SetPropertyAsync("autopilot/settings/target-speed-kt", goalSpeed);
            if (flightPlan.DepartureRunwayId != null && currentWp < 4)
            {
                if (currentWp == 2)
                {
                    await _connectionManager.SetPropertyAsync("autopilot/locks/altitude", "pitch-hold");
                    await _connectionManager.SetPropertyAsync("autopilot/settings/target-pitch-deg", 20);
                }
                else
                {
                    await _connectionManager.SetPropertyAsync("autopilot/locks/altitude", "altitude-hold");
                }
            }

            await Task.Delay(100);
        }
    }

    public async Task ExitSimulationWithPropertySaveAsync()
    {
        if (UserSimulationMasterService.CurrentFlightStatus != FlightStatus.Running || UserSimulationMasterService.CurrentRunningSession == null)
        {
            throw new Exception("Simulation is not running.");
        }

        var session = UserSimulationMasterService.CurrentRunningSession;
        session.DurationSec = (int)(DateTime.Now.ToUniversalTime() - session.DateTimeStart).TotalSeconds;
        await _flightGearLauncher.CloseFlightGearAsync();
        await Task.Delay(1000);
        var scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
        using (var scope = scopeFactory.CreateScope())
        {
            var flightExportedParametersReader = scope.ServiceProvider.GetRequiredService<IFlightExportedParametersReader>();
            var flightPropertiesShotRepository = scope.ServiceProvider.GetRequiredService<IFlightPropertiesShotRepository>();
            var sessionService = scope.ServiceProvider.GetRequiredService<ISessionService>();
            
            var propertiesShots = await flightExportedParametersReader.GetExportedPropertiesAsync(session.Id);
            await flightPropertiesShotRepository.SaveRangeAsync(propertiesShots);
            await sessionService.SaveSessionAsync(session);
        }
        
    }
    
    public async Task<FlightStatus> GetSimulationStatus()
    {
        return UserSimulationMasterService.CurrentFlightStatus;
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
}