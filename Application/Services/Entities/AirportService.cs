using Application.Interfaces.Entities;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Entities;

internal class AirportService : IAirportService
{
    private readonly IRunwayService _runwayService;
    private readonly IServiceProvider _serviceProvider;

    public AirportService(IRunwayService runwayService, IServiceProvider serviceProvider)
    {
        _runwayService = runwayService;
        _serviceProvider = serviceProvider;
    }
    
    public async Task<Airport[]> GetAllAirportsAsync()
    {
        var airportRepository = _serviceProvider.GetRequiredService<IAirportRepository>();
        return await airportRepository.GetAllAirports();
    }

    public async Task<Airport> GetAirportAggregatedAsync(Guid airportId)
    {
        var airportRepository = _serviceProvider.GetRequiredService<IAirportRepository>();
        return await airportRepository.GetAggregateByIdAsync(airportId);
    }

    public async Task SaveAirportAsync(Airport airport)
    {
        var airportRepository = _serviceProvider.GetRequiredService<IAirportRepository>();
        await airportRepository.SaveAsync(airport);
    }

    public async Task DeleteAirportAsync(Guid airportId)
    {
        var airportRepository = _serviceProvider.GetRequiredService<IAirportRepository>();
        await _runwayService.RemoveRunwaysByAirportIdAsync(airportId);
        await airportRepository.RemoveByIdAsync(airportId);
    }
}