using Application.Interfaces.Entities;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Services.Entities;

internal class RunwayService : IRunwayService
{
    private readonly IAirportRunwayRepository _runwayRepository;

    public RunwayService(IAirportRunwayRepository runwayRepository)
    {
        _runwayRepository = runwayRepository;
    }
    
    public async Task SaveRunwayAsync(AirportRunway runway)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveRunwayAsync(AirportRunway runway)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveRunwayByIdAsync(Guid runwayId)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveRunwayByAirportIdAsync(Guid airportId)
    {
        throw new NotImplementedException();
    }

    public async Task<AirportRunway> GetRunwayByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<AirportRunway> GetAggregatedRunwayByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<AirportRunway[]> GetAllRunwaysByAirportId(Guid airportId)
    {
        throw new NotImplementedException();
    }
}