namespace FlightGearApi.Domain.Enums;

public enum ExportProperty
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

public static class ExportPropertyExtensions
{
    public static readonly Dictionary<ExportProperty, (string RussianString, string Path, double Multiplier)> PropertiesInfoDict = new()
    {
        { ExportProperty.Longitude, ("Долгота", "/position/longitude-deg", 1)},
        { ExportProperty.Latitude, ("Широта", "/position/latitude-deg", 1)},
        { ExportProperty.AltitudeAgl, ("Истинная высота", "/position/altitude-agl-ft", 0.3048)},
        { ExportProperty.Altitude, ("Высота", "/position/altitude-ft", 0.3048)},
        { ExportProperty.AltitudeIndicatedBaro, ("Относительная барометрическая высота", "/instrumentation/altimeter/indicated-altitude-ft", 0.3048)},
        { ExportProperty.AltitudeAbsoluteBaro, ("Абсолютная барометрическая высота", "/instrumentation/altimeter/pressure-alt-ft", 0.3048) },
        
        { ExportProperty.Mach, ("Число маха", "/velocities/mach", 1)},
        { ExportProperty.UBodyMps, ("Продольная составляющая земной скорости", "/velocities/uBody-fps", 0.3048)},
        { ExportProperty.VBodyMps, ("Боковая составляющая земной скорости в ЛСК", "/velocities/vBody-fps", 0.3048)},
        { ExportProperty.WBodyMps, ("Вертикальная составляющая земной скорости", "/velocities/wBody-fps", 0.3048)},
        { ExportProperty.Airspeed, ("Воздушная скорость", "/velocities/airspeed-kt", 0.514444)},
        { ExportProperty.VerticalBaroSpeed, ("Вертикальная барометрическая скорость", "/instrumentation/vertical-speed-indicator/indicated-speed-kts", 0.514444)},
        
        { ExportProperty.Roll, ("Крен", "/orientation/roll-deg", 1)},
        { ExportProperty.Pitch, ("Тангаж", "/orientation/pitch-deg", 1)},
        { ExportProperty.Heading, ("Курс", "/orientation/heading-deg", 1)},
        { ExportProperty.HeadingMagnetic, ("Магнитный курс", "/orientation/heading-magnetic-deg", 1)},
        { ExportProperty.HeadingMagneticIndicated, ("Курс гидромагнитный", "/instrumentation/heading-indicator/indicated-heading-deg", 1)},
        
        { ExportProperty.IndicatedSpeed, ("Приборная скорость", "/instrumentation/indicated-speed-kt", 0.514444)},

        { ExportProperty.SideOverload, ("Боковая перегрузка", "/accelerations/pilot-g", 1)},
        { ExportProperty.PilotOverload, ("Перегрузка пилота", "/accelerations/pilot/z-accel-fps_sec", 0.3048)},
        { ExportProperty.AccelerationY, ("Боковое ускороение", "/accelerations/pilot/y-accel-fps_sec", 0.3048)},
        { ExportProperty.AccelerationX, ("Продольное ускороение", "/accelerations/pilot/x-accel-fps_sec", 0.3048)},
        { ExportProperty.AccelerationNormal, ("Нормальное ускороение", "/accelerations/nlf", 0.3048)},
        
        { ExportProperty.Temperature, ("Температура воздуха снаружи C", "/environment/temperature-degc", 1)}
    };
    
    public static string GetRussianVariant(this ExportProperty property)
    {
        if (PropertiesInfoDict.TryGetValue(property, out var result))
        {
            return result.RussianString;
        }

        return "Unknown";
    }
}