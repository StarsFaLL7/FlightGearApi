using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;

namespace Domain.Entities;

public class FlightSession : BaseEntityWithKey<Guid>
{
    public required string Title { get; set; }
    
    public required int PropertiesReadsPerSec { get; set; }
    
    public required DateTime DateTimeStart { get; set; }
    
    public int DurationSec { get; set; }

    [NotMapped] 
    public DateTime DateTimeEnd => DateTimeStart + TimeSpan.FromSeconds(DurationSec);
    
    public ICollection<FlightPropertiesShot> PropertiesCollection { get; set; }
}