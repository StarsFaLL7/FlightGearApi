using FlightGearApi.Domain.Enums;

namespace FlightGearApi.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class PropertyValueAttribute : Attribute
{
    public double Multiplier { get; init; }
    
    public ExportProperty PropertyEnum { get; init; }
    
    public PropertyValueAttribute(ExportProperty propertyEnum, double multiplier = 1)
    {
        Multiplier = multiplier;
        PropertyEnum = propertyEnum;
    }
}