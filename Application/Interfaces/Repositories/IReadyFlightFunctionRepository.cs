using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IReadyFlightFunctionRepository
{
    Task SaveAsync(ReadyFlightFunction function);
    
    Task RemoveByIdAsync(Guid functionId);
    
    Task<ReadyFlightFunction> GetByIdAsync(Guid id);
    
    Task<ReadyFlightFunction> GetAggregateByIdAsync(Guid id);
    
    Task<ReadyFlightFunction[]> GetAllAsync();
}