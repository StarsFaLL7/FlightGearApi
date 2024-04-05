using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Сервис, который будет управлять всеми частями полёта.
/// Все запросы на изменение состояния должны идти через него. Паттерн Gateway.
/// </summary>
public interface IUserSimulationMasterService
{
    /// <summary>
    /// Обновляет существующую или инициализирует новую сущность плана полёта, сохраняет её в системе.
    /// </summary>
    Task<Guid> SaveFlightPlanAsync(FlightPlan flightPlan);

    /// <summary>
    /// Удаляет существующий план полёта из системы.
    /// </summary>
    /// <param name="flightPlanId">Уникальный идентификатор плана полёта.</param>
    Task RemoveFlightPlanAsync(Guid flightPlanId);
    
    /// <summary>
    /// Обновляет существующую или создаёт новую точку плана полёта, сохраняет её в системе.
    /// </summary>
    Task<Guid> SaveRoutePointAsync(RoutePoint routePoint);
    
    /// <summary>
    /// Получить план полёта по его уникальному идентификатору
    /// </summary>
    Task<FlightPlan> GetFlightPlanAsync(Guid flightPlanId);

    /// <summary>
    /// Установить взлётную полосу для взлёта в выбранном плане полёта.
    /// </summary>
    /// <param name="flightPlanId">Уникальный идентификатор плана полёта</param>
    /// <param name="runwayId">Уникальный идентификатор взлётной полосы</param>
    Task SetDepartureRunwayAsync(Guid flightPlanId, Guid runwayId);

    /// <summary>
    /// Установить взлётную полосу для посадки в выбранном плане полёта.
    /// </summary>
    /// <param name="flightPlanId">Уникальный идентификатор плана полёта</param>
    /// <param name="runwayId">Уникальный идентификатор взлётной полосы</param>
    Task SetArrivalRunwayAsync(Guid flightPlanId, Guid runwayId);
    
    /// <summary>
    /// Удалить взлёт с взлётной полосы в выбранном плане полёта.
    /// </summary>
    /// <param name="flightPlanId">Уникальный идентификатор плана полёта</param>
    Task RemoveDepartureRunwayAsync(Guid flightPlanId);

    /// <summary>
    /// Удалить посадку на взлётную полосу в текущем выбранном плане полёта.
    /// </summary>
    /// <param name="flightPlanId">Уникальный идентификатор плана полёта</param>
    Task RemoveArrivalRunwayAsync(Guid flightPlanId);

    /// <summary>
    /// Запустить симуляцию с нужным планом полёта.
    /// </summary>
    /// <param name="flightPlanId">Уникальный идентификатор плана полёта</param>
    /// <param name="flightSession">Сессия, которая будет создана для сохранения параметров.</param>
    Task<Guid> StartSimulationWithFlightPlanAsync(Guid flightPlanId, FlightSession flightSession);
    
    /// <summary>
    /// Получить значение, true - в данный момент симуляция запущена, false - симуляция не запущена.
    /// </summary>
    bool IsSimulationRunningAsync();
    
    /// <summary>
    /// Получить уникальный идентификатор точки, к которой сейчас стремится самолёт.
    /// </summary>
    Task<Guid> GetCurrentGoalRoutePointAsync();
    
    /// <summary>
    /// Получить процент маршрута, который уже был преодолён в текущей симуляции.
    /// </summary>
    Task<int> GetRoutePercentCompletionAsync();

    /// <summary>
    /// Получить значения параметров полёта в текущей запущенной симуляции.
    /// </summary>
    Task<FlightPropertiesShot> GetCurrentFlightValuesAsync();
}