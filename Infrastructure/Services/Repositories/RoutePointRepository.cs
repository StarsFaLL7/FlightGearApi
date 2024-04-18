using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Services.Repositories;

internal class RoutePointRepository : IRoutePointRepository
{
    private readonly PostgresDbContext _dbContext;

    public RoutePointRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task SaveAsync(RoutePoint point)
    {
        if (!_dbContext.RoutePoints.Contains(point))
        {
            _dbContext.RoutePoints.Add(point);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveByIdAsync(Guid pointId)
    {
        var point = _dbContext.RoutePoints.FirstOrDefault(p => p.Id == pointId);
        if (point != null)
        {
            _dbContext.Remove(point);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<RoutePoint> GetByIdAsync(Guid id)
    {
        return _dbContext.RoutePoints.First(p => p.Id == id);
    }

    public async Task<RoutePoint[]> GetAllByFlightPlanId(Guid flightPlanId)
    {
        return _dbContext.RoutePoints.Where(p => p.FlightPlanId == flightPlanId).ToArray();
    }
}