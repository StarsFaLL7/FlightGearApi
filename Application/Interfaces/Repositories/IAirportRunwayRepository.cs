using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IAirportRunwayRepository
{
    Task SaveAsync(AirportRunway runway);

    Task RemoveAsync(AirportRunway runway);
    
    Task RemoveByIdAsync(Guid runwayId);
    
    Task RemoveByAirportIdAsync(Guid airportId);

    Task<AirportRunway> GetByIdAsync(Guid id);
    
    Task<AirportRunway> GetAggregateByIdAsync(Guid id);

    Task<AirportRunway[]> GetAllByAirportId(Guid airportId);
}