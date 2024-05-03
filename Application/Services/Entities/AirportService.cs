using Application.Interfaces.Entities;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Services.Entities;

internal class AirportService : IAirportService
{
    private readonly IAirportRepository _airportRepository;

    public AirportService(IAirportRepository airportRepository)
    {
        _airportRepository = airportRepository;
    }
    
    public async Task<Airport[]> GetAllAirportsAsync()
    {
        return await _airportRepository.GetAllAirports();
    }

    public async Task<Airport> GetAirportAggregatedAsync(Guid airportId)
    {
        return await _airportRepository.GetAggregateByIdAsync(airportId);
    }

    public async Task SaveAirportAsync(Airport airport)
    {
        await _airportRepository.SaveAsync(airport);
    }
}