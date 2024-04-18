using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;

namespace Domain.Entities;

public class FlightPlan : BaseEntityWithKey<Guid>
{
    public required string Title { get; set; }
    
    public string Remarks { get; set; }
    
    public Guid? DepartureRunwayId { get; set; }
    [ForeignKey("DepartureRunwayId")]
    public AirportRunway? DepartureRunway { get; set; }
    
    public Guid? ArrivalRunwayId { get; set; }
    [ForeignKey("ArrivalRunwayId")]
    public AirportRunway? ArrivalRunway { get; set; }
    
    public List<RoutePoint> RoutePoints { get; set; }
}