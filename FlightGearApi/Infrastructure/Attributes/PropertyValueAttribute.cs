namespace FlightGearApi.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class PropertyValueAttribute : Attribute
{
    public double Multiplier { get; init; }
    
    public PropertyValueAttribute(double multiplier = 1)
    {
        Multiplier = multiplier;
    }
}