using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Сервис, который будет управлять получением значений после полёта.
/// Все запросы по аналитике после полёта должны идти через него. Паттерн Gateway.
/// </summary>
public interface IUserAnalyticsMasterService
{
    /// <summary>
    /// Получить все сохранённые сессии полётов.
    /// </summary>
    Task<FlightSession[]> GetAllSavedSessions();

    /// <summary>
    /// Получить все сохранённые значения параметров полёта по уникальному индентификатору сессии.
    /// </summary>
    Task<FlightPropertiesShot[]> GetPropertiesInFlightSession(Guid flightSessionId);
}