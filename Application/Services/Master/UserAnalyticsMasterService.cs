using Application.Interfaces;
using Application.Interfaces.Entities;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Master;

/// <inheritdoc />
internal class UserAnalyticsMasterService : IUserAnalyticsMasterService
{
    private readonly IServiceProvider _serviceProvider;

    public UserAnalyticsMasterService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<FlightSession[]> GetAllSavedSessions()
    {
        var sessionService = _serviceProvider.GetRequiredService<ISessionService>();
        return await sessionService.GetAllSessions();
    }

    public async Task<FlightPropertiesShot[]> GetPropertiesInFlightSession(Guid flightSessionId)
    {
        var sessionService = _serviceProvider.GetRequiredService<ISessionService>();
        
        var session = await sessionService.GetAggregatedSession(flightSessionId);
        return session.PropertiesCollection.ToArray();
    }
}