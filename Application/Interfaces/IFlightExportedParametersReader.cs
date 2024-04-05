using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Сервис для считывания сохранённых параметров полёта из файла экспорта.
/// </summary>
public interface IFlightExportedParametersReader
{
    /// <summary>
    /// Считать параметры полёта в объекты из файла .xml по указанному пути.
    /// </summary>
    Task<FlightPropertiesShot[]> GetExportedPropertiesAsync(string pathToExportFile);
}