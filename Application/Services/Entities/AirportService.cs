using Application.Interfaces.Entities;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Services.Entities;

internal class AirportService : IAirportService
{
    private readonly IAirportRepository _airportRepository;
    private readonly IRunwayService _runwayService;

    public AirportService(IAirportRepository airportRepository, IRunwayService runwayService)
    {
        _airportRepository = airportRepository;
        _runwayService = runwayService;
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

    public async Task DeleteAirportAsync(Guid airportId)
    {
        await _runwayService.RemoveRunwaysByAirportIdAsync(airportId);
        await _airportRepository.RemoveByIdAsync(airportId);
    }
}