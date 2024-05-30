using Application.Enums;
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
    /// Получить значение статуса симуляции полета
    /// </summary>
    Task<FlightStatus> GetSimulationStatus();
    
    /// <summary>
    /// Получить порядковый номер послдней точки маршрута, которую достиг самолет.
    /// </summary>
    Task<int> GetLastReachedRoutePointOrderAsync();
    
    /// <summary>
    /// Получить процент маршрута, который уже был преодолён в текущей симуляции.
    /// </summary>
    Task<int> GetRoutePercentCompletionAsync();

    Task ExitSimulationWithPropertySaveAsync();

    Task FlyCycleAsync(FlightPlan flightPlan);
}