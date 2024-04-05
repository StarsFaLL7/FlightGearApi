using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;

namespace Domain.Entities;

public class FunctionPoint : BaseEntityWithKey<Guid>
{
    public required int Order { get; set; }
    
    public required double Longitude { get; set; }
    
    public required double Latitude { get; set; }
    
    public required double Altitude { get; set; }
    
    public required double Speed { get; set; }
    
    public string? Remarks { get; set; }
    
    public Guid FunctionId { get; set; }
    [ForeignKey("FunctionId")]
    public ReadyFlightFunction Function { get; set; }
}