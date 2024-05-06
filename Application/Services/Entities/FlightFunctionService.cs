using Application.Interfaces.Entities;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Services.Entities;

public class FlightFunctionService : IFlightFunctionService
{
    private readonly IFunctionPointRepository _functionPointRepository;
    private readonly IReadyFlightFunctionRepository _flightFunctionRepository;

    public FlightFunctionService(IFunctionPointRepository functionPointRepository, IReadyFlightFunctionRepository flightFunctionRepository)
    {
        _functionPointRepository = functionPointRepository;
        _flightFunctionRepository = flightFunctionRepository;
    }
    
    public async Task SaveFunction(ReadyFlightFunction flightFunction)
    {
        await _flightFunctionRepository.SaveAsync(flightFunction);
    }

    public async Task RemoveFunctionById(Guid id)
    {
        //await _functionPointRepository.RemoveAllByFunctionIdAsync(id);
        await _flightFunctionRepository.RemoveByIdAsync(id);
    }

    public async Task SaveFunctionPointRange(FunctionPoint[] points)
    {
        foreach (var point in points)
        {
            await _functionPointRepository.SaveAsync(point);
        }
        
    }
}