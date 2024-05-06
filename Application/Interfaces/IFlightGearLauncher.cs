using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Сервис для запуска симуляции FlightGear
/// </summary>
public interface IFlightGearLauncher
{
    /// <summary>
    /// Инициализация параметров запуска в соответствие с планом полёта. План должен быть сохранён в Базе данных.
    /// </summary>
    Task InitializeWithFlightPlanAsync(FlightPlan flightPlan);
    
    /// <summary>
    /// Запуск симуляции
    /// </summary>
    Task TryLaunchSimulationAsync(FlightSession flightSession);

    /// <summary>
    /// Завершение симуляции
    /// </summary>
    Task CloseFlightGearAsync();
    
    /// <summary>
    /// Получить строку запуска FlightGear со всеми параметрами. Нужно для отладки.
    /// </summary>
    string GetLaunchString(int propertiesReadsPerSecond);

    string GetExportedTextDataFilePath();
}