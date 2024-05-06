using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Services.Repositories;

internal class FunctionPointRepository : IFunctionPointRepository
{
    private readonly PostgresDbContext _dbContext;

    public FunctionPointRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task SaveAsync(FunctionPoint point)
    {
        if (!_dbContext.FunctionPoints.Contains(point))
        {
            _dbContext.FunctionPoints.Add(point);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task SaveRangeAsync(FunctionPoint[] points)
    {
        await _dbContext.FunctionPoints.AddRangeAsync(points);

        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveByIdAsync(Guid pointId)
    {
        var point = _dbContext.FunctionPoints.FirstOrDefault(p => p.Id == pointId);
        if (point != null)
        {
            _dbContext.FunctionPoints.Remove(point);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task RemoveAllByFunctionIdAsync(Guid functionId)
    {
        var pointsToDelete = _dbContext.FunctionPoints.Where(p => p.FunctionId == functionId);
        _dbContext.FunctionPoints.RemoveRange(pointsToDelete);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<FunctionPoint> GetByIdAsync(Guid id)
    {
        return _dbContext.FunctionPoints.First(p => p.Id == id);
    }

    public async Task<FunctionPoint[]> GetAllByFunctionId(Guid functionId)
    {
        return _dbContext.FunctionPoints.Where(p => p.FunctionId == functionId).ToArray();
    }
}