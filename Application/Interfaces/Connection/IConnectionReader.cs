using Domain.Entities;
using Domain.Enums.FlightUtilityProperty;

namespace Application.Interfaces.Connection;

/// <summary>
/// Сервис для получения текущих значений параметров из запущенной симуляции.
/// </summary>
public interface IConnectionReader
{
    /// <summary>
    /// Получить текущие значения полёта для utility свойств.
    /// </summary>
    /// <param name="properties">Свойства, значения которых нужно получить</param>
    /// <returns></returns>
    Task<Dictionary<string, double>> GetCurrentUtilityValuesAsync(params FlightUtilityProperty[] properties);
    
    /// <summary>
    /// Получить текущие значения полёта.
    /// </summary>
    /// <returns></returns>
    Task<FlightPropertiesShot> GetCurrentValuesAsync();
    
    /// <summary>
    /// Получить значение свойства по заданному пути из PropertyTree.
    /// </summary>
    /// <param name="propertyPath">Путь к свойству (вида "/position/longitude-deg")</param>
    /// <returns>Значение свойства, конвертированное в тип double</returns>
    Task<double> GetPropertyDoubleValueAsync(string propertyPath);
    
    /// <summary>
    /// Получить значение свойства по заданному пути из PropertyTree.
    /// </summary>
    /// <param name="propertyPath">Путь к свойству (вида "/position/longitude-deg")</param>
    /// <returns>Значение свойства</returns>
    Task<string?> GetPropertyStringValueAsync(string propertyPath);
}