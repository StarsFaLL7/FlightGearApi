using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IFlightSavedSessionsRepository
{
    Task SaveAsync(FlightSession session);

    Task RemoveByIdAsync(Guid sessionId);
    
    Task<FlightSession> GetAggregateByIdAsync(Guid sessionId);
    
    Task<FlightSession[]> GetAllSessionsAsync();
}