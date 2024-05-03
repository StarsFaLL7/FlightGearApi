using Application.Enums;
using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Сервис, который будет управлять всеми частями полёта.
/// Все запросы на изменение состояния симуляции должны идти через него.
/// </summary>
public interface IUserSimulationMasterService
{
    /// <summary>
    /// Запустить симуляцию с нужным планом полёта.
    /// </summary>
    /// <param name="flightPlanId">Уникальный идентификатор плана полёта</param>
    /// <param name="flightSession">Сессия, в которую будут сохраняться параметры.</param>
    Task StartSimulationWithFlightPlanAsync(Guid flightPlanId, FlightSession flightSession);

    /// <summary>
    /// Досрочный выход из симуляции
    /// </summary>
    Task ExitSimulationAsync();
    
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

    /// <summary>
    /// Получить значения параметров полёта в текущей запущенной симуляции.
    /// </summary>
    Task<FlightPropertiesShot> GetCurrentFlightValuesAsync();
}