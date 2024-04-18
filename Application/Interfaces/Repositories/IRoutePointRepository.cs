using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IRoutePointRepository
{
    Task SaveAsync(RoutePoint point);
    
    Task RemoveByIdAsync(Guid pointId);

    Task<RoutePoint> GetByIdAsync(Guid id);

    Task<RoutePoint[]> GetAllByFlightPlanId(Guid flightPlanId);
}