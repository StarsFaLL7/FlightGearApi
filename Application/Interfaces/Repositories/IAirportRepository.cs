using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IAirportRepository
{
    Task SaveAsync(Airport airport);

    Task RemoveAsync(Airport airport);
    
    Task RemoveByIdAsync(Guid airportId);

    Task<Airport> GetByIdAsync(Guid id);
    
    Task<Airport> GetAggregateByIdAsync(Guid id);

    Task<Airport[]> GetAllAirports();
}