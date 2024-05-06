using Domain.Entities;

namespace Application.Dto;

/// <summary>
/// Модель для передачи из слоя Application. Содержит точки маршрута, которые были конвертированы из функций взлета, посадки.
/// </summary>
public class FlightPlanAggregated
{
    public required Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public string? Remarks { get; set; }
    
    public required AirportRunway? DepartureRunway { get; set; }
    
    public required AirportRunway? ArrivalRunway { get; set; }
    
    public required List<RoutePointWithIsEditableProperty> RoutePoints { get; set; }
}