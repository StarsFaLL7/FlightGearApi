using Application.Enums;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Services.Master;

/// <inheritdoc />
internal class UserSimulationMasterService : IUserSimulationMasterService
{
    private readonly IFlightGearLauncher _flightGearLauncher;
    private readonly IFlightPlanRepository _flightPlanRepository;

    private FlightStatus _currentStatus = FlightStatus.NotRunning;
    
    public UserSimulationMasterService(IFlightGearLauncher flightGearLauncher, IFlightPlanRepository flightPlanRepository)
    {
        _flightGearLauncher = flightGearLauncher;
        _flightPlanRepository = flightPlanRepository;
    }
    
    public async Task StartSimulationWithFlightPlanAsync(Guid flightPlanId, FlightSession flightSession)
    {
        _currentStatus = FlightStatus.Launching;
        try
        {
            var flightPlan = await _flightPlanRepository.GetAggregateByIdAsync(flightPlanId);
            await _flightGearLauncher.InitializeWithFlightPlanAsync(flightPlan);
            await _flightGearLauncher.TryLaunchSimulationAsync(flightSession);
            _currentStatus = FlightStatus.Running;
        }
        catch (Exception e)
        {
            _currentStatus = FlightStatus.Exited;
            throw;
        }
    }

    public async Task ExitSimulationAsync()
    {
        await _flightGearLauncher.ExitSimulationAsync();
    }

    public async Task<FlightStatus> GetSimulationStatus()
    {
        return _currentStatus;
    }

    public async Task<int> GetLastReachedRoutePointOrderAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<int> GetRoutePercentCompletionAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<FlightPropertiesShot> GetCurrentFlightValuesAsync()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Запуск цикла обновления для своевременного выхода из симуляции. Цикл крутится всё время, пока идёт симуляция.
    /// </summary>
    private async Task UpdateCycle()
    {
        throw new NotImplementedException();
    }
}