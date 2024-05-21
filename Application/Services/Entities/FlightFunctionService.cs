using Application.Interfaces.Entities;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Entities;

public class FlightFunctionService : IFlightFunctionService
{
    private readonly IServiceProvider _serviceProvider;

    public FlightFunctionService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task SaveFunction(ReadyFlightFunction flightFunction)
    {
        var flightFunctionRepository = _serviceProvider.GetRequiredService<IReadyFlightFunctionRepository>();
        await flightFunctionRepository.SaveAsync(flightFunction);
    }

    public async Task RemoveFunctionById(Guid id)
    {
        var flightFunctionRepository = _serviceProvider.GetRequiredService<IReadyFlightFunctionRepository>();
        await flightFunctionRepository.RemoveByIdAsync(id);
    }

    public async Task SaveFunctionPointRange(FunctionPoint[] points)
    {
        var functionPointRepository = _serviceProvider.GetRequiredService<IFunctionPointRepository>();
        foreach (var point in points)
        {
            await functionPointRepository.SaveAsync(point);
        }
        
    }
}