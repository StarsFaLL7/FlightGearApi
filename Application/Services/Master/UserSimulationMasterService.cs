using Application.Interfaces;
using Domain.Entities;

namespace Application.Services.Master;

/// <inheritdoc />
internal class UserSimulationMasterService : IUserSimulationMasterService
{
    
    public UserSimulationMasterService()
    {
        
    }

    public async Task<Guid> SaveFlightPlanAsync(FlightPlan flightPlan)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveFlightPlanAsync(Guid flightPlanId)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> SaveRoutePointAsync(RoutePoint routePoint)
    {
        throw new NotImplementedException();
    }

    public async Task<FlightPlan> GetFlightPlanAsync(Guid flightPlanId)
    {
        throw new NotImplementedException();
    }

    public async Task SetDepartureRunwayAsync(Guid flightPlanId, Guid runwayId)
    {
        throw new NotImplementedException();
    }

    public async Task SetArrivalRunwayAsync(Guid flightPlanId, Guid runwayId)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveDepartureRunwayAsync(Guid flightPlanId)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveArrivalRunwayAsync(Guid flightPlanId)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> StartSimulationWithFlightPlanAsync(Guid flightPlanId, FlightSession flightSession)
    {
        throw new NotImplementedException();
    }

    public bool IsSimulationRunningAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> GetCurrentGoalRoutePointAsync()
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