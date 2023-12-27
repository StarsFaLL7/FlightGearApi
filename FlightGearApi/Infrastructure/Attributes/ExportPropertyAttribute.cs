using FlightGearApi.Domain.Enums;

namespace FlightGearApi.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ExportPropertyAttribute : Attribute
{
    public ExportProperty PropertyEnum { get; init; }
    
    public ExportPropertyAttribute(ExportProperty propertyEnum)
    {
        PropertyEnum = propertyEnum;
    }
}