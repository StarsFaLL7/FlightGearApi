using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IFlightPlanRepository
{
    Task SaveAsync(FlightPlan plan);
    
    Task RemoveByIdAsync(Guid planId);

    Task<FlightPlan> GetByIdAsync(Guid id);
    
    Task<FlightPlan> GetAggregateByIdAsync(Guid id);
}