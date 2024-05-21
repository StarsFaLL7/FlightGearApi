using Application.Interfaces.Entities;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Entities;

internal class RunwayService : IRunwayService
{
    private readonly IServiceProvider _serviceProvider;

    public RunwayService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task SaveRunwayAsync(AirportRunway runway)
    {
        var runwayRepository = _serviceProvider.GetRequiredService<IAirportRunwayRepository>();
        await runwayRepository.SaveAsync(runway);
    }

    public async Task RemoveRunwayByIdAsync(Guid runwayId)
    {
        var runwayRepository = _serviceProvider.GetRequiredService<IAirportRunwayRepository>();
        var functionPointRepository = _serviceProvider.GetRequiredService<IFunctionPointRepository>();
        var functionRepository = _serviceProvider.GetRequiredService<IReadyFlightFunctionRepository>();
        
        var runway = await GetRunwayByIdAsync(runwayId);
        if (runway.DepartureFunctionId != null)
        {
            await functionPointRepository.RemoveAllByFunctionIdAsync(runway.DepartureFunctionId.Value);
            await functionRepository.RemoveByIdAsync(runway.DepartureFunctionId.Value);
        }
        if (runway.ArrivalFunctionId != null)
        {
            await functionPointRepository.RemoveAllByFunctionIdAsync(runway.ArrivalFunctionId.Value);
            await functionRepository.RemoveByIdAsync(runway.ArrivalFunctionId.Value);
        }
        await runwayRepository.RemoveByIdAsync(runwayId);
    }

    public async Task RemoveRunwaysByAirportIdAsync(Guid airportId)
    {
        var runwayRepository = _serviceProvider.GetRequiredService<IAirportRunwayRepository>();
        var functionPointRepository = _serviceProvider.GetRequiredService<IFunctionPointRepository>();
        var airportRepository = _serviceProvider.GetRequiredService<IAirportRepository>();
        var functionRepository = _serviceProvider.GetRequiredService<IReadyFlightFunctionRepository>();
        
        var airport = await airportRepository.GetAggregateByIdAsync(airportId);
        foreach (var runway in airport.Runways)
        {
            if (runway.DepartureFunctionId != null)
            {
                await functionPointRepository.RemoveAllByFunctionIdAsync(runway.DepartureFunctionId.Value);
                await functionRepository.RemoveByIdAsync(runway.DepartureFunctionId.Value);
            }
            if (runway.ArrivalFunctionId != null)
            {
                await functionPointRepository.RemoveAllByFunctionIdAsync(runway.ArrivalFunctionId.Value);
                await functionRepository.RemoveByIdAsync(runway.ArrivalFunctionId.Value);
            }
        }
        await runwayRepository.RemoveByAirportIdAsync(airportId);
    }

    public async Task<AirportRunway> GetRunwayByIdAsync(Guid id)
    {
        var runwayRepository = _serviceProvider.GetRequiredService<IAirportRunwayRepository>();
        return await runwayRepository.GetByIdAsync(id);
    }

    public async Task<AirportRunway> GetAggregatedRunwayByIdAsync(Guid id)
    {
        var runwayRepository = _serviceProvider.GetRequiredService<IAirportRunwayRepository>();
        return await runwayRepository.GetAggregateByIdAsync(id);
    }

    public async Task<AirportRunway[]> GetAllRunwaysByAirportId(Guid airportId)
    {
        var runwayRepository = _serviceProvider.GetRequiredService<IAirportRunwayRepository>();
        return await runwayRepository.GetAllByAirportId(airportId);
    }

    public async Task<AirportRunway[]> GetAggregatedRunwaysByAirportId(Guid airportId)
    {
        var runwayRepository = _serviceProvider.GetRequiredService<IAirportRunwayRepository>();
        return await runwayRepository.GetAggregatedRunwaysByAirportId(airportId);
    }
}