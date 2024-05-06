using Domain.Entities;

namespace Application.Interfaces.Entities;

public interface ISessionService
{
    Task<FlightSession[]> GetAllSessions();
    
    Task<FlightSession> GetAggregatedSession(Guid sessionId);
    
    Task SaveSessionAsync(FlightSession session);
    
    Task RemoveSessionAsync(Guid sessionId);
}