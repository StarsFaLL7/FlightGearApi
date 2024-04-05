namespace Domain.Enums.FlightExportProperty;

public enum FlightExportProperty
{
    Longitude = 1,
    Latitude = 2,
    AltitudeAgl = 3, // Истинная
    Altitude = 4,
    AltitudeIndicatedBaro = 5,
    
    Mach = 6,
    UBodyMps = 7,
    VBodyMps = 8,
    WBodyMps = 9,
    Airspeed = 10,
    VerticalBaroSpeed = 11,
    
    Roll = 12,
    Pitch = 13,
    Heading = 14,
    HeadingMagnetic = 15,
    HeadingMagneticIndicated = 16,
    
    IndicatedSpeed = 17, // Приборная скорость
    
    PilotOverload = 18,
    AccelerationY = 19,
    AccelerationX = 20,
    AccelerationNormal = 21,
    
    Temperature = 22
}