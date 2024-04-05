using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Сервис для управления полётом. 
/// </summary>
public interface IFlightManipulator
{
    /// <summary>
    /// Инициализировать настройки в соответствии с планом полёта.
    /// </summary>
    Task InitializeAsync(FlightPlan flightPlan);
    
    /// <summary>
    /// Запустить цикл контроля полёта. Цикл крутится всё время, пока самолёт летит к последней точке.
    /// </summary>
    Task FlyCycleAsync();

    /// <summary>
    /// Получить кол-во точек, до которых ещё предстоить лететь.
    /// </summary>
    /// <returns></returns>
    Task<int> GetLeftPointsToAchieveAsync();
    
    /// <summary>
    /// Получить процент, насколько маршрут завершён.
    /// </summary>
    Task GetPercentRouteCompletionAsync();
    
    /// <summary>
    /// Закончить управление полётом.
    /// </summary>
    void EndFlight();
}