using Domain.Entities;

namespace Application.Interfaces.Entities;
/// <summary>
/// Сервис для работы с аэропортами и взлетными, посадочными полосами
/// </summary>
public interface IAirportService
{
    /// <summary>
    /// Получить все аэропорты. (Базовая информация)
    /// </summary>
    /// <returns>Не аггрегированные модели аэропортов.</returns>
    Task<Airport[]> GetAllAirportsAsync();
    
    /// <summary>
    /// Получить аэропорт по его уникальному идентификатору.
    /// </summary>
    /// <returns>Аггрегированные модели аэропортов.</returns>
    Task<Airport> GetAirportAggregatedAsync(Guid airportId);
    
    /// <summary>
    /// Сохранить аэропорт. Если он не существует в базе данных, он будет добавлен.
    /// </summary>
    Task SaveAirportAsync(Airport airport);
}