using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using FlightGearApi.Domain.Enums;
using FlightGearApi.Domain.Records;
using FlightGearApi.Domain.UtilityClasses;

namespace FlightGearApi.Domain.FlightGearCore;

public class ConnectionListener
{
    private IoManager IoManager { get; }

    private readonly Dictionary<string, List<FgMessage>> _listenResults = new ();
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
        _listenResults.Add(ListeningSessionName, new List<FgMessage>());
        
        IsListeningForClient = true;
        
        ListenCycleForClient(ListeningSessionName);
    }
    
    private async void ListenCycleForClient(string name)
    {
        while (true)
        {
            if (!IsListeningForClient || IoManager.OutputPropertiesList.Count == 0)
            {
                IsListeningForClient = false;
                return;
            }
            
            try
            {
                var result = await GetCurrentValuesAsync();
                _listenResults[name].Add(new FgMessage() {Date = DateTime.Now, Values = result});
                
                await Task.Delay((int)(1000 / IoManager.ConnectionRefreshesPerSecond));
            }
            catch (Exception e)
            {
                IsListeningForClient = false;
                return;
            }
        }
    }

    public void StopListenForClient()
    {
        IsListeningForClient = false;
        // SAVE TO DB
    }

    public async Task<Dictionary<string, double>> GetCurrentValuesAsync(bool utility = false)
    {
        if (!utility && IoManager.OutputPropertiesList.Count == 0)
        {
            return new();
        }
        
        var result = new Dictionary<string, double>();
        
        var propertiesList = utility 
            ? FlightPropertiesHelper.AllUtilityProperties() 
            : IoManager.OutputPropertiesList;
        
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
                var value = ParseDoubleFromResponse(response);
                result[property.Name] = value;
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
        
        var match = Regex.Match(response, @"'-?([\d.]+)'");

        if (match.Success)
        {
            var valueString = match.Groups[1].Value;

            if (double.TryParse(valueString, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }
        }
        throw new ArgumentException("Invalid response provided.");
    }
    
    public void ClearResults()
    {
        _listenResults.Clear();
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