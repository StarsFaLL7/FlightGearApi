using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Attributes;
using Domain.Base;
using Domain.Enums.FlightExportProperty;

namespace Domain.Entities;

[Table("flight_properties_shots")]
public class FlightPropertiesShot : BaseEntityWithKey<Guid>
{
    public required int Order { get; set; }
    
    public required Guid FlightSessionId { get; set; }
    [ForeignKey("FlightSessionId")] 
    public FlightSession FlightSession { get; set; }
    
    [FlightPropertyInfo(FlightExportProperty.Longitude)]
    public required double Longitude { get; set; }
    
    [FlightPropertyInfo(FlightExportProperty.Latitude)]
    public required double Latitude { get; set; }
    
    [FlightPropertyInfo(FlightExportProperty.AltitudeAgl, 0.3048)]
    public required double AltitudeAgl { get; set; } // Истинная
    
    [FlightPropertyInfo(FlightExportProperty.Altitude,0.3048)]
    public required double Altitude { get; set; }
    
    [FlightPropertyInfo(FlightExportProperty.AltitudeIndicatedBaro, 0.3048)]
    public required double AltitudeIndicatedBaro { get; set; }
    
    [FlightPropertyInfo(FlightExportProperty.Roll)]
    public required double Roll { get; set; }
    
    [FlightPropertyInfo(FlightExportProperty.Pitch)]
    public required double Pitch { get; set; }
    
    [FlightPropertyInfo(FlightExportProperty.Heading)]
    public required double Heading { get; set; }
    
    [FlightPropertyInfo(FlightExportProperty.HeadingMagnetic)]
    public required double HeadingMagnetic { get; set; }
    
    [FlightPropertyInfo(FlightExportProperty.HeadingMagneticIndicated)]
    public required double HeadingMagneticIndicated { get; set; }
    
    [FlightPropertyInfo(FlightExportProperty.IndicatedSpeed, 0.514444)]
    public required double IndicatedSpeed { get; set; } // Приборная скорость
    
    [FlightPropertyInfo(FlightExportProperty.Airspeed, 0.514444)]
    public required double Airspeed { get; set; }
    
    [FlightPropertyInfo(FlightExportProperty.VerticalBaroSpeed, 0.514444)]
    public required double VerticalBaroSpeed { get; set; }
    
    [FlightPropertyInfo(FlightExportProperty.Mach)]
    public required double Mach { get; set; }
    
    [FlightPropertyInfo(FlightExportProperty.UBodyMps, 0.3048)]
    public required double UBodyMps { get; set; }
    
    [FlightPropertyInfo(FlightExportProperty.VBodyMps, 0.3048)]
    public required double VBodyMps { get; set; }
    
    [FlightPropertyInfo(FlightExportProperty.WBodyMps, 0.3048)]
    public required double WBodyMps { get; set; }
    
    [FlightPropertyInfo(FlightExportProperty.PilotOverload, 0.3048)]
    public required double PilotOverload { get; set; }
    
    [FlightPropertyInfo(FlightExportProperty.AccelerationY, 0.3048)]
    public required double AccelerationY { get; set; }
    
    [FlightPropertyInfo(FlightExportProperty.AccelerationX, 0.3048)]
    public required double AccelerationX { get; set; }
    
    [FlightPropertyInfo(FlightExportProperty.AccelerationNormal, 0.3048)]
    public required double AccelerationNormal { get; set; }
    
    [FlightPropertyInfo(FlightExportProperty.Temperature)]
    public required double Temperature { get; set; }
}