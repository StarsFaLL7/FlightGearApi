using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IFunctionPointRepository
{
    Task SaveAsync(FunctionPoint point);
    
    Task SaveRangeAsync(FunctionPoint[] point);
    
    Task RemoveByIdAsync(Guid pointId);
    
    Task RemoveAllByFunctionIdAsync(Guid functionId);

    Task<FunctionPoint> GetByIdAsync(Guid id);

    Task<FunctionPoint[]> GetAllByFunctionId(Guid functionId);
}