using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class FlightGearLauncher : IFlightGearLauncher
{
    public async Task InitializeWithFlightPlanAsync(FlightPlan flightPlan)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> TryLaunchSimulationAsync()
    {
        throw new NotImplementedException();
    }

    public void Exit()
    {
        throw new NotImplementedException();
    }

    public string GetLaunchString()
    {
        throw new NotImplementedException();
    }
}