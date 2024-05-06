using Application.Dto;
using Domain.Entities;

namespace Application.Interfaces.Entities;

/// <summary>
/// Сервис для работы с планами полета
/// </summary>
public interface IFlightPlanService
{
    /// <summary>
    /// Получение базовой информации о всех сохраненный планах полета
    /// </summary>
    Task<FlightPlan[]> GetAllBasicFlightPlansInfosAsync();
    
    /// <summary>
    /// Получить аггрегированную модель плана полёта по его уникальному идентификатору
    /// </summary>
    Task<FlightPlan> GetAggregatedFlightPlanAsync(Guid flightPlanId);
    
    /// <summary>
    /// Получить аггрегированную модель плана полёта по его уникальному идентификатору, с точками маршрута, полученными из функций взлета, посадки
    /// </summary>
    Task<FlightPlanAggregated> GetFlightPlanWithConvertedPointsAsync(Guid flightPlanId);
    
    /// <summary>
    /// Обновляет существующую или инициализирует новую сущность плана полёта, сохраняет её в системе.
    /// </summary>
    Task<FlightPlan> SaveFlightPlanAsync(FlightPlan flightPlan);

    /// <summary>
    /// Удаляет существующий план полёта из системы.
    /// </summary>
    /// <param name="flightPlanId">Уникальный идентификатор плана полёта.</param>
    Task RemoveFlightPlanAsync(Guid flightPlanId);
    
    /// <summary>
    /// Обновляет существующую точку плана полёта.
    /// </summary>
    Task UpdateRoutePointAsync(Guid flightPlanId, int pointOrder, 
        double longitude, double latitude, double altitude, string? remarks);
    
    /// <summary>
    /// Обновляет существующую или создаёт новую точку плана полёта, сохраняет её в системе.
    /// </summary>
    Task SaveRoutePointAsync(RoutePoint routePoint);
    
    /// <summary>
    /// Удаление точки маршрута у существующего плана полета
    /// </summary>
    Task RemoveRoutePointAsync(Guid flightPlanId, int pointOrder);

    Task RemoveDepartureRunwayFromFlightPlansByRunwayId(Guid runwayId);
    
    Task RemoveArrivalRunwayFromFlightPlansByRunwayId(Guid runwayId);
}