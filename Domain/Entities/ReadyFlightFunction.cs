using Domain.Base;

namespace Domain.Entities;

public class ReadyFlightFunction : BaseEntityWithKey<Guid>
{
    public string? Description { get; set; }
    
    public ICollection<FunctionPoint> FunctionPoints { get; set; }
}