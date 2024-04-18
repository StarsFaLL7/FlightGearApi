using Application.Interfaces;
using Application.Interfaces.Connection;
using Domain.Entities;

namespace Application.Services;

public class FlightManipulator : IFlightManipulator
{
    private readonly IConnectionManager _connectionManager;
    public bool IsRunning { get; set; }
    public FlightManipulator(IConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    public async Task InitializeAsync(FlightPlan flightPlan)
    {
        throw new NotImplementedException();
    }

    public async Task FlyCycleAsync()
    {
        IsRunning = true;
        while (IsRunning)
        {
            
        }
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