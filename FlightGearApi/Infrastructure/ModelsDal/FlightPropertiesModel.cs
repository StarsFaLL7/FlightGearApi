using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    
    [PropertyValue]
    public double Longitude { get; set; }
    
    [PropertyValue]
    public double Latitude { get; set; }
    
    [PropertyValue(0.3048)]
    public double AltitudeAgl { get; set; } // Истинная
    
    [PropertyValue(0.3048)]
    public double Altitude { get; set; }
    
    [PropertyValue(0.3048)]
    public double AltitudeIndicatedBaro { get; set; }
    
    [PropertyValue(0.3048)]
    public double AltitudeAbsoluteBaro { get; set; }
    
    [PropertyValue]
    public double Roll { get; set; }
    
    [PropertyValue]
    public double Pitch { get; set; }
    
    [PropertyValue]
    public double Heading { get; set; }
    
    [PropertyValue]
    public double HeadingMagnetic { get; set; }
    
    [PropertyValue]
    public double HeadingMagneticIndicated { get; set; }
    
    [PropertyValue(0.514444)]
    public double IndicatedSpeed { get; set; } // Приборная скорость
    
    [PropertyValue(0.514444)]
    public double Airspeed { get; set; }
    
    [PropertyValue(0.514444)]
    public double VerticalBaroSpeed { get; set; }
    
    [PropertyValue]
    public double Mach { get; set; }
    
    [PropertyValue(0.3048)]
    public double UBodyMps { get; set; }
    
    [PropertyValue(0.3048)]
    public double VBodyMps { get; set; }
    
    [PropertyValue(0.3048)]
    public double WBodyMps { get; set; }
    
    [PropertyValue]
    public double SideOverload { get; set; }
    
    [PropertyValue(0.3048)]
    public double PilotOverload { get; set; }
    
    [PropertyValue(0.3048)]
    public double AccelerationY { get; set; }
    
    [PropertyValue(0.3048)]
    public double AccelerationX { get; set; }
    
    [PropertyValue(0.3048)]
    public double AccelerationNormal { get; set; }
    
    [PropertyValue]
    public double Temperature { get; set; }
}