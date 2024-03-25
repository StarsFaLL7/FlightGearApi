namespace Domain.Enums.FlightExportProperty;

public enum FlightExportProperty
{
    Longitude,
    Latitude,
    AltitudeAgl, // Истинная
    Altitude,
    AltitudeIndicatedBaro,
    AltitudeAbsoluteBaro,
    Roll,
    Pitch,
    Heading,
    HeadingMagnetic,
    HeadingMagneticIndicated,
    IndicatedSpeed, // Приборная скорость
    Airspeed,
    VerticalBaroSpeed,
    Mach,
    UBodyMps,
    VBodyMps,
    WBodyMps,
    SideOverload,
    PilotOverload,
    Temperature,
    AccelerationY,
    AccelerationX,
    AccelerationNormal
}