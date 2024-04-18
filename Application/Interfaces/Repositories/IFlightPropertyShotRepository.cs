using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IFlightPropertiesShotRepository
{
    Task SaveRangeAsync(params FlightPropertiesShot[] shots);
    
    Task<FlightPropertiesShot[]> GetAllBySessionIdAsync(Guid sessionId);
    
}