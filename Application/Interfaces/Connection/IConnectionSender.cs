using Domain.Enums.FlightUtilityProperty;

namespace Application.Interfaces.Connection;

/// <summary>
/// Сервис для установки значений свойств в FG.
/// </summary>
public interface IConnectionSender
{
    /// <summary>
    /// Установить указанные значения соответствующим параметрам.
    /// </summary>
    Task SetParametersAsync(Dictionary<FlightUtilityProperty, double> propertiesToChange);
    
    /// <summary>
    /// Установить значени для свойства по заданному пути из propertyTree.
    /// </summary>
    /// <param name="propertyPath">Путь к свойству (вида "/position/longitude-deg")</param>
    /// <param name="value">Значение, которое нужно установить. Будет конвертировано в строку через ToString()</param>
    Task SetPropertyAsync(string propertyPath, object value);
    
    /// <summary>
    /// Отправить команду в FlightGear через Telnet.
    /// </summary>
    /// <param name="command">Команда, которая будет отправлена.</param>
    /// <param name="readResponse">Нужно ли возвращать ответ, который выведет FlightGear после получения команды.</param>
    /// <returns>Ответ от FlightGear, если readResponse = true, иначе null</returns>
    Task<string?> SendCommandAsync(string command, bool readResponse);
}