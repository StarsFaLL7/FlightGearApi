using Domain.Entities;

namespace Application.Interfaces.Entities;

public interface IFlightFunctionService
{
    Task SaveFunction(ReadyFlightFunction flightFunction);

    Task RemoveFunctionById(Guid id);
    
    Task SaveFunctionPointRange(FunctionPoint[] points);
}