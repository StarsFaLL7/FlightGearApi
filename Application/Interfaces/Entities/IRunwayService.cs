using Domain.Entities;

namespace Application.Interfaces.Entities;

/// <summary>
/// Сервис для работы с взлетными/посадочными полосами
/// </summary>
public interface IRunwayService
{
    Task SaveRunwayAsync(AirportRunway runway);
    
    Task RemoveRunwayByIdAsync(Guid runwayId);
    
    Task RemoveRunwaysByAirportIdAsync(Guid airportId);

    Task<AirportRunway> GetRunwayByIdAsync(Guid id);
    
    Task<AirportRunway> GetAggregatedRunwayByIdAsync(Guid id);

    Task<AirportRunway[]> GetAllRunwaysByAirportId(Guid airportId);
    
    Task<AirportRunway[]> GetAggregatedRunwaysByAirportId(Guid airportId);
}