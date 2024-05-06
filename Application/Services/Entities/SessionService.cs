using Application.Interfaces.Entities;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Services.Entities;

public class SessionService : ISessionService
{
    private readonly IFlightSavedSessionsRepository _sessionsRepository;
    private readonly IFlightPropertiesShotRepository _propertiesShotRepository;

    public SessionService(IFlightSavedSessionsRepository sessionsRepository, IFlightPropertiesShotRepository propertiesShotRepository)
    {
        _sessionsRepository = sessionsRepository;
        _propertiesShotRepository = propertiesShotRepository;
    }
    
    public async Task<FlightSession[]> GetAllSessions()
    {
        return await _sessionsRepository.GetAllSessionsAsync();
    }

    public async Task<FlightSession> GetAggregatedSession(Guid sessionId)
    {
        return await _sessionsRepository.GetAggregateByIdAsync(sessionId);
    }

    public async Task SaveSessionAsync(FlightSession session)
    {
        if (session.PropertiesReadsPerSec < 1)
        {
            throw new ArgumentException($"Invalid value for session.PropertiesReadsPerSecond = {session.PropertiesReadsPerSec}");
        }
        await _sessionsRepository.SaveAsync(session);
    }

    public async Task RemoveSessionAsync(Guid sessionId)
    {
        await _propertiesShotRepository.RemoveBySessionIdAsync(sessionId);
        await _sessionsRepository.RemoveByIdAsync(sessionId);
    }
}