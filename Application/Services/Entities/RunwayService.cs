using Application.Interfaces.Entities;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Services.Entities;

internal class RunwayService : IRunwayService
{
    private readonly IAirportRunwayRepository _runwayRepository;
    private readonly IFunctionPointRepository _functionPointRepository;
    private readonly IReadyFlightFunctionRepository _functionRepository;
    private readonly IAirportRepository _airportRepository;

    public RunwayService(IAirportRunwayRepository runwayRepository, IFunctionPointRepository functionPointRepository,
        IReadyFlightFunctionRepository functionRepository, IAirportRepository airportRepository)
    {
        _runwayRepository = runwayRepository;
        _functionPointRepository = functionPointRepository;
        _functionRepository = functionRepository;
        _airportRepository = airportRepository;
    }
    
    public async Task SaveRunwayAsync(AirportRunway runway)
    {
        await _runwayRepository.SaveAsync(runway);
    }

    public async Task RemoveRunwayByIdAsync(Guid runwayId)
    {
        var runway = await GetRunwayByIdAsync(runwayId);
        if (runway.DepartureFunctionId != null)
        {
            await _functionPointRepository.RemoveAllByFunctionIdAsync(runway.DepartureFunctionId.Value);
            await _functionRepository.RemoveByIdAsync(runway.DepartureFunctionId.Value);
        }
        if (runway.ArrivalFunctionId != null)
        {
            await _functionPointRepository.RemoveAllByFunctionIdAsync(runway.ArrivalFunctionId.Value);
            await _functionRepository.RemoveByIdAsync(runway.ArrivalFunctionId.Value);
        }
        await _runwayRepository.RemoveByIdAsync(runwayId);
    }

    public async Task RemoveRunwaysByAirportIdAsync(Guid airportId)
    {
        var airport = await _airportRepository.GetAggregateByIdAsync(airportId);
        foreach (var runway in airport.Runways)
        {
            if (runway.DepartureFunctionId != null)
            {
                await _functionPointRepository.RemoveAllByFunctionIdAsync(runway.DepartureFunctionId.Value);
                await _functionRepository.RemoveByIdAsync(runway.DepartureFunctionId.Value);
            }
            if (runway.ArrivalFunctionId != null)
            {
                await _functionPointRepository.RemoveAllByFunctionIdAsync(runway.ArrivalFunctionId.Value);
                await _functionRepository.RemoveByIdAsync(runway.ArrivalFunctionId.Value);
            }
        }
        await _runwayRepository.RemoveByAirportIdAsync(airportId);
    }

    public async Task<AirportRunway> GetRunwayByIdAsync(Guid id)
    {
        return await _runwayRepository.GetByIdAsync(id);
    }

    public async Task<AirportRunway> GetAggregatedRunwayByIdAsync(Guid id)
    {
        return await _runwayRepository.GetAggregateByIdAsync(id);
    }

    public async Task<AirportRunway[]> GetAllRunwaysByAirportId(Guid airportId)
    {
        return await _runwayRepository.GetAllByAirportId(airportId);
    }

    public async Task<AirportRunway[]> GetAggregatedRunwaysByAirportId(Guid airportId)
    {
        return await _runwayRepository.GetAggregatedRunwaysByAirportId(airportId);
    }
}