using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class FlightManipulator : IFlightManipulator
{
    public FlightManipulator()
    {
    }

    public async Task InitializeAsync(FlightPlan flightPlan)
    {
        throw new NotImplementedException();
    }

    public async Task FlyCycleAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<int> GetLeftPointsToAchieveAsync()
    {
        throw new NotImplementedException();
    }

    public async Task GetPercentRouteCompletionAsync()
    {
        throw new NotImplementedException();
    }

    public void EndFlight()
    {
        throw new NotImplementedException();
    }
}