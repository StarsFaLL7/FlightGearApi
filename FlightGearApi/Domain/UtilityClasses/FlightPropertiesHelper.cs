using FlightGearApi.Domain.Records;

namespace FlightGearApi.Domain.UtilityClasses;

public enum UtilityProperty
{
    Throttle, // Мощность двигателя
    Aileron, // Наклон штурвала
    Elevator, // Положение штурвала (на/от себя)
    Rudder, // Прокрутка самолёта для управления рысканием
    SpeedBrake, // Воздушные тормозы
    ParkingBrake,
    Altitude, // Высота в метрах
    Roll, // Крен
    Pitch, // Тангаж
    Heading, // Курс
    VerticalSpeed,
    IndicatedSpeed
}

public static class FlightPropertiesHelper
{
    public static Dictionary<UtilityProperty, FlightPropertyInfo> OutputProperties { get; } = new ()
    {
        { UtilityProperty.Altitude, new FlightPropertyInfo(
            "/position/altitude-agl-ft", 
            "altitude-agl-ft", 
            typeof(double),
            "double",
            "%.5f") },
        { UtilityProperty.Roll, new FlightPropertyInfo(
            "/orientation/roll-deg", 
            "roll-deg", 
            typeof(double),
            "double",
            "%.5f") },
        { UtilityProperty.Pitch, new FlightPropertyInfo(
            "/orientation/pitch-deg", 
            "pitch-deg", 
            typeof(double),
            "double",
            "%.5f") },
        { UtilityProperty.Heading, new FlightPropertyInfo(
            "/orientation/heading-deg", 
            "heading-deg", 
            typeof(double),
            "double",
            "%.5f") },
        { UtilityProperty.VerticalSpeed , new FlightPropertyInfo(
            "/velocities/vertical-speed-fps",
            "vertical-speed-fps",
            typeof(double),
            "double",
            "%.5f") },
        { UtilityProperty.IndicatedSpeed , new FlightPropertyInfo(
            "/instrumentation/airspeed-indicator/indicated-speed-kt",
            "indicated-speed-kt",
            typeof(double),
            "double",
            "%.5f") },
    };

    public static Dictionary<UtilityProperty, (FlightPropertyInfo Property, double MinValue, double MaxValue)> InputProperties { get; } = new()
    {
        { UtilityProperty.Throttle, (new FlightPropertyInfo(
            "/controls/engines/current-engine/throttle",
            "throttle", 
            typeof(double),
            "double",
            "%.5f"), 
            0, 1)},
        { UtilityProperty.Aileron, (new FlightPropertyInfo(
            "/controls/flight/aileron",
            "aileron", 
            typeof(double),
            "double",
            "%.5f"), 
            -1, 1)},
        { UtilityProperty.Elevator, (new FlightPropertyInfo(
            "/controls/flight/elevator",
            "elevator", 
            typeof(double),
            "double",
            "%.5f"), 
            -1, 1)},
        { UtilityProperty.Rudder, (new FlightPropertyInfo(
            "/controls/flight/rudder",
            "rudder", 
            typeof(double),
            "double",
            "%.5f"), 
            -1, 1)},
        { UtilityProperty.SpeedBrake, (new FlightPropertyInfo(
                "/controls/flight/speedbrake",
                "speedbrake", 
                typeof(double),
                "double",
                "%.5f"), 
            0, 1)},
        { UtilityProperty.ParkingBrake, (new FlightPropertyInfo(
                "/controls/gear/brake-parking",
                "brake-parking", 
                typeof(double),
                "double",
                "%.5f"), 
            0, 1)}
    };

    public static List<FlightPropertyInfo> AllUtilityPropertiesList()
    {
        return InputProperties
            .Select(p => p.Value.Property)
            .Concat(OutputProperties.Values)
            .ToList();
    }

    public static Dictionary<UtilityProperty, FlightPropertyInfo> AllUtilityPropertiesDict
    {
        get
        {
            var result = new Dictionary<UtilityProperty, FlightPropertyInfo>(OutputProperties);
            foreach (var pairValue in InputProperties)
            {
                result[pairValue.Key] = pairValue.Value.Property;
            }
            return result;
        }
    }
    
    public static UtilityProperty ConvertNameToUtilityProperty(string name)
    {
        var stringValue = name.Split('-')[0];
        if (Enum.TryParse<UtilityProperty>(stringValue, true, out var result))
        {
            return result;
        }

        throw new Exception("Convert from string to enum UtilityProperty failed.");
    }

    public static string GetName(this UtilityProperty property)
    {
        return OutputProperties.TryGetValue(property, out var outputProperty) ? 
            outputProperty.Name : 
            InputProperties[property].Property.Name;
    }
}