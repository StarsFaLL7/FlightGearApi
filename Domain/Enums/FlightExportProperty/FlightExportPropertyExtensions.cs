namespace Domain.Enums.FlightExportProperty;

public static class FlightExportPropertyExtensions
{
    public static readonly Dictionary<FlightExportProperty, (string RussianString, string Path, double Multiplier)> PropertiesInfoDict = new()
    {
        { FlightExportProperty.Longitude, ("Долгота", "/position/longitude-deg", 1)},
        { FlightExportProperty.Latitude, ("Широта", "/position/latitude-deg", 1)},
        { FlightExportProperty.AltitudeAgl, ("Истинная высота", "/position/altitude-agl-ft", 0.3048)},
        { FlightExportProperty.Altitude, ("Высота", "/position/altitude-ft", 0.3048)},
        { FlightExportProperty.AltitudeIndicatedBaro, ("Относительная барометрическая высота", "/instrumentation/altimeter/indicated-altitude-ft", 0.3048)},
        { FlightExportProperty.AltitudeAbsoluteBaro, ("Абсолютная барометрическая высота", "/instrumentation/altimeter/pressure-alt-ft", 0.3048) },
        
        { FlightExportProperty.Mach, ("Число маха", "/velocities/mach", 1)},
        { FlightExportProperty.UBodyMps, ("Продольная составляющая земной скорости", "/velocities/uBody-fps", 0.3048)},
        { FlightExportProperty.VBodyMps, ("Боковая составляющая земной скорости в ЛСК", "/velocities/vBody-fps", 0.3048)},
        { FlightExportProperty.WBodyMps, ("Вертикальная составляющая земной скорости", "/velocities/wBody-fps", 0.3048)},
        { FlightExportProperty.Airspeed, ("Воздушная скорость", "/velocities/airspeed-kt", 0.514444)},
        { FlightExportProperty.VerticalBaroSpeed, ("Вертикальная барометрическая скорость", "/instrumentation/vertical-speed-indicator/indicated-speed-kts", 0.514444)},
        
        { FlightExportProperty.Roll, ("Крен", "/orientation/roll-deg", 1)},
        { FlightExportProperty.Pitch, ("Тангаж", "/orientation/pitch-deg", 1)},
        { FlightExportProperty.Heading, ("Курс", "/orientation/heading-deg", 1)},
        { FlightExportProperty.HeadingMagnetic, ("Магнитный курс", "/orientation/heading-magnetic-deg", 1)},
        { FlightExportProperty.HeadingMagneticIndicated, ("Курс гидромагнитный", "/instrumentation/heading-indicator/indicated-heading-deg", 1)},
        
        { FlightExportProperty.IndicatedSpeed, ("Приборная скорость", "/instrumentation/indicated-speed-kt", 0.514444)},

        { FlightExportProperty.SideOverload, ("Боковая перегрузка", "/accelerations/pilot-g", 1)},
        { FlightExportProperty.PilotOverload, ("Перегрузка пилота", "/accelerations/pilot/z-accel-fps_sec", 0.3048)},
        { FlightExportProperty.AccelerationY, ("Боковое ускороение", "/accelerations/pilot/y-accel-fps_sec", 0.3048)},
        { FlightExportProperty.AccelerationX, ("Продольное ускороение", "/accelerations/pilot/x-accel-fps_sec", 0.3048)},
        { FlightExportProperty.AccelerationNormal, ("Нормальное ускороение", "/accelerations/nlf", 0.3048)},
        
        { FlightExportProperty.Temperature, ("Температура воздуха снаружи C", "/environment/temperature-degc", 1)}
    };
    
    public static string GetRussianVariant(this FlightExportProperty exportProperty)
    {
        if (PropertiesInfoDict.TryGetValue(exportProperty, out var result))
        {
            return result.RussianString;
        }

        return "Unknown";
    }
}