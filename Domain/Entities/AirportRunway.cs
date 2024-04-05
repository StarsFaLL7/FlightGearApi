using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;

namespace Domain.Entities;

public class AirportRunway : BaseEntityWithKey<Guid>
{
    public required string Title { get; set; }
    
    public Guid AirportId { get; set; }
    [ForeignKey("AirportId")]
    public Airport Airport { get; set; }
    
    public Guid DepartureFunctionId { get; set; }
    [ForeignKey("DepartureFunctionId")]
    public ReadyFlightFunction DepartureFunction { get; set; }
    
    public Guid ArrivalFunctionId { get; set; }
    [ForeignKey("ArrivalFunctionId")]
    public ReadyFlightFunction ArrivalFunction { get; set; }
}