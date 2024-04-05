using Domain.Enums.FlightUtilityProperty;
using Domain.Utility;

namespace Domain.ValueObjects;

public class FlightPropertyInfo
{
    public string Path { get; }
    public string Name { get; }
    public string TypeName { get; }
    public double Multiplier { get; }

    public static FlightPropertyInfo FromUtilityProperty(FlightUtilityProperty utilityProperty)
    {
        FlightPropertyInfo model;
        if (FlightUtilityPropertiesHelper.InputProperties.TryGetValue(utilityProperty, out var info))
        {
            model = info.Property;
        }
        else
        {
            model = FlightUtilityPropertiesHelper.OutputProperties[utilityProperty];
        }

        var result = new FlightPropertyInfo(model.Path, model.Name, model.TypeName, model.Multiplier);
        return result;
    }
    
    public FlightPropertyInfo(string path, string name, string typeName, double multiplier = 1)
    {
        Name = name;
        TypeName = typeName;
        Multiplier = multiplier;
        Path = path;
    }
}