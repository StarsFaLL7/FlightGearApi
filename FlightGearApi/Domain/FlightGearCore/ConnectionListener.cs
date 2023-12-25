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
using FlightGearApi.Domain.Records;
using FlightGearApi.Domain.UtilityClasses;

namespace FlightGearApi.Domain.FlightGearCore;

public class ConnectionListener
{
    private IoManager IoManager { get; }

    public readonly Dictionary<string, List<FgMessage>> ListenResults = new ();
    private string ListeningSessionName { get; set; } = "test1";
    
    public bool IsFlightGearRunning { get; set; }
    
    public bool IsListeningForClient { get; set; }
    
    public ConnectionListener(IoManager ioManager)
    {
        IoManager = ioManager;
    }
    
    public void StartListenForClient(string sessionName)
    {
        if (IsListeningForClient)
        {
            return;
        }

        ListeningSessionName = sessionName;
        ListenResults.Add(ListeningSessionName, new List<FgMessage>());
        
        IsListeningForClient = true;
        
        ListenCycleForClient(ListeningSessionName);
    }
    
    private async void ListenCycleForClient(string name)
    {
        var exceptionsCount = 0;
        while (true)
        {
            if (exceptionsCount > 10)
            {
                IsListeningForClient = false;
                return;
            }
            if (!IsListeningForClient || IoManager.OutputPropertiesList.Count == 0)
            {
                IsListeningForClient = false;
                return;
            }
            IsListeningForClient = true;
            try
            {
                var result = await GetCurrentValuesAsync();
                ListenResults[name].Add(new FgMessage() {Date = DateTime.Now, Values = result});
                
                await Task.Delay((int)(1000 / IoManager.ConnectionRefreshesPerSecond));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                exceptionsCount++;
            }
        }
    }

    public void StopListenForClient()
    {
        IsListeningForClient = false;
        // SAVE TO DB
    }

    public async Task<Dictionary<string, double>> GetCurrentValuesAsync(bool utility = false, params UtilityProperty[] onlyProperties)
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
                var value = ParseDoubleFromResponse(response) * property.Multifier;
                result[property.Name] = value;
            }
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
    
    // Использовался при получения данных по UDP
    /*
    private Dictionary<string, string> GenerateDictFromMessageOld(string message)
    {
        var result = new Dictionary<string, string>();
        var lines = message.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            var pair = line.Split('=');
            result.Add(pair[0], pair[1]);
        }

        return result;
    }
*/
}

public class FgMessage
{
    public DateTime Date { get; init; }
    public Dictionary<string, double> Values { get; init; } = new ();
}