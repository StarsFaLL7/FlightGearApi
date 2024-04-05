using Domain.Enums.FlightUtilityProperty;
using Domain.ValueObjects;

namespace Domain.Utility;

public static class FlightUtilityPropertiesHelper
{
    public static Dictionary<FlightUtilityProperty, FlightPropertyInfo> OutputProperties { get; } = new ()
    {
        { FlightUtilityProperty.Altitude, new FlightPropertyInfo(
            "/position/altitude-ft", 
            "altitude-ft",
            "double", 
            0.304785) },
        { FlightUtilityProperty.Roll, new FlightPropertyInfo(
            "/orientation/roll-deg", 
            "roll-deg",
            "double") },
        { FlightUtilityProperty.Pitch, new FlightPropertyInfo(
            "/orientation/pitch-deg", 
            "pitch-deg",
            "double") },
        { FlightUtilityProperty.Heading, new FlightPropertyInfo(
            "/orientation/heading-deg", 
            "heading-deg",
            "double") },
        { FlightUtilityProperty.VerticalSpeed , new FlightPropertyInfo(
            "/velocities/vertical-speed-fps",
            "vertical-speed-fps",
            "double", 
            0.304785) },
        { FlightUtilityProperty.IndicatedSpeed , new FlightPropertyInfo(
            "/instrumentation/airspeed-indicator/indicated-speed-kt",
            "indicated-speed-kt",
            "double", 
            0.514444) },
    };

    public static Dictionary<FlightUtilityProperty, (FlightPropertyInfo Property, double MinValue, double MaxValue)> InputProperties { get; } = new()
    {
        { FlightUtilityProperty.Throttle, (new FlightPropertyInfo(
            "/controls/engines/current-engine/throttle",
            "throttle",
            "double"), 
            0, 1)},
        { FlightUtilityProperty.Aileron, (new FlightPropertyInfo(
            "/controls/flight/aileron",
            "aileron",
            "double"), 
            -1, 1)},
        { FlightUtilityProperty.Elevator, (new FlightPropertyInfo(
            "/controls/flight/elevator",
            "elevator",
            "double"), 
            -1, 1)},
        { FlightUtilityProperty.Rudder, (new FlightPropertyInfo(
            "/controls/flight/rudder",
            "rudder",
            "double"), 
            -1, 1)},
        { FlightUtilityProperty.SpeedBrake, (new FlightPropertyInfo(
                "/controls/flight/speedbrake",
                "speedbrake",
                "double"), 
            0, 1)},
        { FlightUtilityProperty.ParkingBrake, (new FlightPropertyInfo(
                "/controls/gear/brake-parking",
                "brake-parking",
                "double"), 
            0, 1)},
        { FlightUtilityProperty.ApPitchSwitch, (new FlightPropertyInfo(
                "/autopilot/KAP140/locks/pitch-axis",
                "pitch-axis",
                "double"), 
            0, 1)},
        { FlightUtilityProperty.ApHeadingSwitch, (new FlightPropertyInfo(
                "/autopilot/KAP140/locks/hdg-hold",
                "hdg-hold",
                "double"), 
            0, 1)},
        { FlightUtilityProperty.ApRollSwitch, (new FlightPropertyInfo(
                "/autopilot/KAP140/locks/roll-axis",
                "roll-axis", 
                "double"), 
            0, 1)},
        { FlightUtilityProperty.ApTargetVerticalPressureRate, (new FlightPropertyInfo(
                "/autopilot/KAP140/settings/target-pressure-rate",
                "target-pressure-rate", 
                "double"), 
            -1, 1)},
        { FlightUtilityProperty.ApHeadingHeadingDeg, (new FlightPropertyInfo(
                "/autopilot/settings/heading-bug-deg",
                "heading-bug-deg",
                "double"), 
            0, 360)},
        { FlightUtilityProperty.Flaps, (new FlightPropertyInfo(
                "/controls/flight/flaps",
                "flaps", 
                "double"
                ), 
            0, 1)}
    };
    
    public static List<FlightPropertyInfo> AllUtilityPropertiesList()
    {
        return InputProperties
            .Select(p => p.Value.Property)
            .Concat(OutputProperties.Values)
            .ToList();
    }

    public static FlightPropertyInfo GetInfo(this FlightUtilityProperty property)
    {
        return OutputProperties.TryGetValue(property, out var outputProperty) ? 
            outputProperty : 
            InputProperties[property].Property;
    }
}