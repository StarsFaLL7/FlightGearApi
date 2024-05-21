using Application.Interfaces.Entities;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Entities;

public class SessionService : ISessionService
{
    private readonly IServiceProvider _serviceProvider;
    public SessionService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<FlightSession[]> GetAllSessions()
    {
        var sessionsRepository = _serviceProvider.GetRequiredService<IFlightSavedSessionsRepository>();
        return await sessionsRepository.GetAllSessionsAsync();
    }

    public async Task<FlightSession> GetAggregatedSession(Guid sessionId)
    {
        var sessionsRepository = _serviceProvider.GetRequiredService<IFlightSavedSessionsRepository>();
        return await sessionsRepository.GetAggregateByIdAsync(sessionId);
    }

    public async Task SaveSessionAsync(FlightSession session)
    {
        if (session.PropertiesReadsPerSec < 1)
        {
            throw new ArgumentException($"Invalid value for session.PropertiesReadsPerSecond = {session.PropertiesReadsPerSec}");
        }
        var sessionsRepository = _serviceProvider.GetRequiredService<IFlightSavedSessionsRepository>();
        await sessionsRepository.SaveAsync(session);
    }

    public async Task RemoveSessionAsync(Guid sessionId)
    {
        var sessionsRepository = _serviceProvider.GetRequiredService<IFlightSavedSessionsRepository>();
        var propertiesShotRepository = _serviceProvider.GetRequiredService<IFlightPropertiesShotRepository>();
        await propertiesShotRepository.RemoveBySessionIdAsync(sessionId);
        await sessionsRepository.RemoveByIdAsync(sessionId);
    }
}