using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FlightGearApi.Domain.Enums;
using FlightGearApi.Domain.Logging;
using FlightGearApi.Domain.Records;
using FlightGearApi.Domain.UtilityClasses;
using FlightGearApi.Infrastructure.ModelsDal;

namespace FlightGearApi.Domain.FlightGearCore;

public class ConnectionListener
{
    private IoManager IoManager { get; }

    public readonly Dictionary<string, List<FlightPropertiesModel>> ListenResults = new ();
    
    public bool IsFlightGearRunning { get; set; }
    
    
    public ConnectionListener(IoManager ioManager)
    {
        IoManager = ioManager;
    }

    public async Task<Dictionary<string, double>> GetCurrentValuesTelnetAsync(bool utility = false, params UtilityProperty[] onlyProperties)
    {
        // TODO: Завернуть все попытки подключения в try {} except
        if (!utility && IoManager.OutputPropertiesList.Count == 0)
        {
            return new();
        }
        
        var result = new Dictionary<string, double>();
        
        var propertiesList = utility 
            ? FlightPropertiesHelper.AllUtilityPropertiesList() 
            : IoManager.OutputPropertiesList;

        if (onlyProperties.Length != 0)
        {
            propertiesList = GetFlightPropertyInfos(onlyProperties);
        }

        try
        {
            using (var client = new TcpClient("127.0.0.1", IoManager.TelnetPort))
            using (var stream = client.GetStream())
            using (var writer = new StreamWriter(stream, Encoding.ASCII))
            using (var reader = new StreamReader(stream, Encoding.ASCII))
            {
                foreach (var property in propertiesList)
                {
                    await writer.WriteLineAsync($"get {property.Path}");
                    await writer.FlushAsync();
                    var response = await reader.ReadLineAsync();
                    var value = ParseDoubleFromResponse(response) * property.Multiplier;
                    result[property.Name] = value;
                }
            }
        }
        catch (Exception e)
        {
            await StaticLogger.LogAsync(LogLevel.Error, $"Error while trying to get currentValues by Telnet in ConnectionListener {e}");
        }
        return result;
    }

    private List<FlightPropertyInfo> GetFlightPropertyInfos(UtilityProperty[] utilityProperties)
    {
        var result = new List<FlightPropertyInfo>();
        foreach (var utilityProperty in utilityProperties)
        {
            FlightPropertiesHelper.OutputProperties.TryGetValue(utilityProperty, out var property);
            if (FlightPropertiesHelper.InputProperties.TryGetValue(utilityProperty, out var propertyTuple))
            {
                property = propertyTuple.Property;
            }
                
            if (property != null && !result.Contains(property))
            {
                result.Add(property);
            }
        }

        return result;
    }

    private double ParseDoubleFromResponse(string response)
    {
        if (string.IsNullOrWhiteSpace(response))
        {
            throw new ArgumentException("Invalid response provided.");
        }

        var commaIndex = response.IndexOf('\'')+1;

        var valueString =
            response.Substring(commaIndex, response.LastIndexOf('\'') - commaIndex);

        if (double.TryParse(valueString, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
        {
            return Math.Round(result, 5);
        }

        if (bool.TryParse(valueString, out var resultBool))
        {
            return resultBool ? 1 : 0;
        }

        return 0;
    }
    
    public void ClearResults()
    {
        ListenResults.Clear();
    }
    
}