using FlightGearApi.records;

namespace FlightGearApi.UtilityClasses;

public enum FlightProperties
{
    Throttle,
    AltitudeMeters
}

public static class FlightPropertiesInfo
{
    public static Dictionary<FlightProperties, FlightPropertyInfo> Properties { get; } = new ()
    {
        { FlightProperties.Throttle, new FlightPropertyInfo(
                "/controls/engines/current-engine/throttle",
                "Throttle In", 
                typeof(double),
                "double",
                "%.5f")
        },
        { FlightProperties.AltitudeMeters, new FlightPropertyInfo(
            "/position/altitude-agl-m", 
            "Altitude in Meters", 
            typeof(double),
            "double",
            "%.5f")
        }
    };
}