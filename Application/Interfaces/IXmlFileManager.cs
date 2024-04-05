using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Менеджер XML файлов, используется для создания, записи xml-файлов для FlightGear
/// </summary>
public interface IXmlFileManager
{
    /// <summary>
    /// Создаёт или обновляет xml-файл, в котором задаются настройки считвания параметров из FG
    /// </summary>
    Task CreateOrUpdateExportXmlFileAsync(int readsPerSecond);
    
    /// <summary>
    /// Создаёт или обновляет xml-файл, в котором задаются настройки маршрута для FG
    /// </summary>
    Task CreateOrUpdateRouteManagerXmlFileAsync(FlightPlan flightPlan);
}