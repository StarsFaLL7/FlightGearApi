using Domain.Enums.FlightExportProperty;

namespace Domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class FlightPropertyInfoAttribute : Attribute
{
    public double Multiplier { get; init; }
    
    public FlightExportProperty ExportPropertyEnum { get; init; }
    
    public FlightPropertyInfoAttribute(FlightExportProperty exportPropertyEnum, double multiplier = 1)
    {
        Multiplier = multiplier;
        ExportPropertyEnum = exportPropertyEnum;
    }
}