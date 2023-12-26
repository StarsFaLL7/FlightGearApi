using System.Globalization;
using System.Reflection;
using System.Text;
using FlightGearApi.Infrastructure.Attributes;
using FlightGearApi.Infrastructure.ModelsDal;

namespace FlightGearApi.Domain.FlightGearCore;

public class ExportParametersManager
{
    private readonly IConfiguration _configuration;
    
    public ExportParametersManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public List<FlightPropertiesModel> GetExportedProperties(int sessionId)
    {
        var filepath = $"{_configuration.GetSection("FlightGear:ExportPropertiesFilePath").Value}";
        var lines = File.ReadAllLines(filepath, Encoding.UTF8);
        var result = new List<FlightPropertiesModel>();
        var order = 0;
        foreach (var line in lines)
        {
            var valuesStr = line.Split(';');
            var propertyShot = new FlightPropertiesModel() {Order = order, FlightSessionId = sessionId};
            
            var propertiesInfos = propertyShot.GetType().GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(PropertyValueAttribute)))
                .ToArray();
            
            for (var i = 0; i < propertiesInfos.Length; i++)
            {
                var str = valuesStr[i];
                var value = ParseDoubleFromString(str);
                var propertyInfo = propertiesInfos[i];
                var attribute = propertyInfo.GetCustomAttribute<PropertyValueAttribute>();
                var multiplier = attribute?.Multiplier ?? 1;
                propertyInfo.SetValue(propertyShot, value * multiplier);
            }
            result.Add(propertyShot);
            order++;
        }

        return result;
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
    
}