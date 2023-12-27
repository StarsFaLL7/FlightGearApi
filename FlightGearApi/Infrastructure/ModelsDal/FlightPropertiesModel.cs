using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlightGearApi.Domain.Enums;
using FlightGearApi.Infrastructure.Attributes;

namespace FlightGearApi.Infrastructure.ModelsDal;

[Table("flight_properties_shots")]
public class FlightPropertiesModel
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("flight_session_id")]
    public int FlightSessionId { get; set; }

    [Column("order")]
    public int Order { get; set; }
    
    [ForeignKey("FlightSessionId")] 
    public FlightSessionDal FlightSession { get; set; }
    
    [PropertyValue(ExportProperty.Longitude)]
    public double Longitude { get; set; }
    
    [PropertyValue(ExportProperty.Latitude)]
    public double Latitude { get; set; }
    
    [PropertyValue(ExportProperty.AltitudeAgl, 0.3048)]
    public double AltitudeAgl { get; set; } // Истинная
    
    [PropertyValue(ExportProperty.Altitude,0.3048)]
    public double Altitude { get; set; }
    
    [PropertyValue(ExportProperty.AltitudeIndicatedBaro, 0.3048)]
    public double AltitudeIndicatedBaro { get; set; }
    
    [PropertyValue(ExportProperty.AltitudeAbsoluteBaro,0.3048)]
    public double AltitudeAbsoluteBaro { get; set; }
    
    [PropertyValue(ExportProperty.Roll)]
    public double Roll { get; set; }
    
    [PropertyValue(ExportProperty.Pitch)]
    public double Pitch { get; set; }
    
    [PropertyValue(ExportProperty.Heading)]
    public double Heading { get; set; }
    
    [PropertyValue(ExportProperty.HeadingMagnetic)]
    public double HeadingMagnetic { get; set; }
    
    [PropertyValue(ExportProperty.HeadingMagneticIndicated)]
    public double HeadingMagneticIndicated { get; set; }
    
    [PropertyValue(ExportProperty.IndicatedSpeed, 0.514444)]
    public double IndicatedSpeed { get; set; } // Приборная скорость
    
    [PropertyValue(ExportProperty.Airspeed, 0.514444)]
    public double Airspeed { get; set; }
    
    [PropertyValue(ExportProperty.VerticalBaroSpeed, 0.514444)]
    public double VerticalBaroSpeed { get; set; }
    
    [PropertyValue(ExportProperty.Mach)]
    public double Mach { get; set; }
    
    [PropertyValue(ExportProperty.UBodyMps, 0.3048)]
    public double UBodyMps { get; set; }
    
    [PropertyValue(ExportProperty.VBodyMps, 0.3048)]
    public double VBodyMps { get; set; }
    
    [PropertyValue(ExportProperty.WBodyMps, 0.3048)]
    public double WBodyMps { get; set; }
    
    [PropertyValue(ExportProperty.SideOverload)]
    public double SideOverload { get; set; }
    
    [PropertyValue(ExportProperty.PilotOverload, 0.3048)]
    public double PilotOverload { get; set; }
    
    [PropertyValue(ExportProperty.AccelerationY, 0.3048)]
    public double AccelerationY { get; set; }
    
    [PropertyValue(ExportProperty.AccelerationX, 0.3048)]
    public double AccelerationX { get; set; }
    
    [PropertyValue(ExportProperty.AccelerationNormal, 0.3048)]
    public double AccelerationNormal { get; set; }
    
    [PropertyValue(ExportProperty.Temperature)]
    public double Temperature { get; set; }
}