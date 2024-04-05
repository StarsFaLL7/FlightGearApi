using Domain.Enums.FlightUtilityProperty;

namespace Application.Interfaces.Connection;

/// <summary>
/// Сервис для установки значений свойств в FG
/// </summary>
public interface IConnectionSender
{
    /// <summary>
    /// Установить указанные значения соответствующим параметрам.
    /// </summary>
    Task SendParametersAsync(Dictionary<FlightUtilityProperty, double> propertiesToChange);
}