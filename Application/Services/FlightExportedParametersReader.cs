using System.Globalization;
using System.Reflection;
using Application.Interfaces;
using Domain.Attributes;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class FlightExportedParametersReader : IFlightExportedParametersReader
{
    private readonly string _pathToExportDataFile;

    public FlightExportedParametersReader(IFlightGearLauncher launcher)
    {
        _pathToExportDataFile = launcher.GetExportedTextDataFilePath();
    }

    public async Task<FlightPropertiesShot[]> GetExportedPropertiesAsync(Guid flightSessionId)
    {
        var result = new List<FlightPropertiesShot>();
        var propertiesInfos = typeof(FlightPropertiesShot).GetProperties()
            .Where(p => Attribute.IsDefined(p, typeof(FlightPropertyInfoAttribute)))
            .ToArray();
        using (var reader = new StreamReader(_pathToExportDataFile))
        {
            string? line;
            var order = 0;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                var valuesStr = line.Split(';');
                var propertyShot = CreateNewShot(order, flightSessionId);
                
                for (var i = 0; i < propertiesInfos.Length; i++)
                {
                    var str = valuesStr[i];
                    var value = ParseDoubleFromString(str);
                    var propertyInfo = propertiesInfos[i];
                    var attribute = propertyInfo.GetCustomAttribute<FlightPropertyInfoAttribute>();
                    var multiplier = attribute?.Multiplier ?? 1;
                    propertyInfo.SetValue(propertyShot, value * multiplier);
                }

                result.Add(propertyShot);
                order++;
            }
        }

        return result.ToArray();
    }
    
    private double ParseDoubleFromString(string str)
    {
        if (double.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
        {
            return Math.Round(result, 5);
        }

        if (bool.TryParse(str, out var resultBool))
        {
            return resultBool ? 1 : 0;
        }

        return 0;
    }

    private FlightPropertiesShot CreateNewShot(int order, Guid sessionId)
    {
        return new FlightPropertiesShot
        {
            Order = order,
            FlightSessionId = sessionId,
            Longitude = 0,
            Latitude = 0,
            AltitudeAgl = 0,
            Altitude = 0,
            AltitudeIndicatedBaro = 0,
            Roll = 0,
            Pitch = 0,
            Heading = 0,
            HeadingMagnetic = 0,
            HeadingMagneticIndicated = 0,
            IndicatedSpeed = 0,
            Airspeed = 0,
            VerticalBaroSpeed = 0,
            Mach = 0,
            UBodyMps = 0,
            VBodyMps = 0,
            WBodyMps = 0,
            PilotOverload = 0,
            AccelerationY = 0,
            AccelerationX = 0,
            AccelerationNormal = 0,
            Temperature = 0,
            Id = Guid.NewGuid()
        };
    }
}