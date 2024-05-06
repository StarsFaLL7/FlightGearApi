using Application.Interfaces;
using Application.Interfaces.Entities;
using Domain.Entities;

namespace Application.Services.Master;

/// <inheritdoc />
internal class UserAnalyticsMasterService : IUserAnalyticsMasterService
{
    private readonly ISessionService _sessionService;

    public UserAnalyticsMasterService(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }
    
    public async Task<FlightSession[]> GetAllSavedSessions()
    {
        return await _sessionService.GetAllSessions();
    }

    public async Task<FlightPropertiesShot[]> GetPropertiesInFlightSession(Guid flightSessionId)
    {
        var session = await _sessionService.GetAggregatedSession(flightSessionId);
        return session.PropertiesCollection.ToArray();
    }
}