using Application.Interfaces;
using Domain.Entities;

namespace Application.Services.Master;

/// <inheritdoc />
internal class UserAnalyticsMasterService : IUserAnalyticsMasterService
{
    public UserAnalyticsMasterService()
    {
        
    }
    
    public async Task<FlightSession[]> GetAllSavedSessions()
    {
        throw new NotImplementedException();
    }

    public async Task<FlightPropertiesShot[]> GetPropertiesInFlightSession(Guid flightSessionId)
    {
        throw new NotImplementedException();
    }
}