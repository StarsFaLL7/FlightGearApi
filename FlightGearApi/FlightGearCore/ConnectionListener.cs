using System.Net.Sockets;
using System.Text;
using FlightGearApi.Enums;
using FlightGearApi.records;

namespace FlightGearApi.FlightGearCore;

public class ConnectionListener
{
    private Dictionary<string, (UdpClient Client, GenericConnectionInfo ConnenctionInfo)> _listenSessions = new ();

    private Dictionary<string, List<FgMessage>> _listenResults = new ();
    
    public void StartListen(GenericConnectionInfo connectionInfo, string name)
    {
        if (connectionInfo.IoType == IoType.Input)
        {
            return;
        }

        if (_listenSessions.Any(p => p.Value.ConnenctionInfo.Port == connectionInfo.Port))
        {
            return;
        }
        var udpClient = new UdpClient(connectionInfo.Port);
        _listenSessions.Add(name, (udpClient, connectionInfo));
        _listenResults.Add(name, new List<FgMessage>());
        ListenAsync(name);
    }
    
    private async void ListenAsync(string name)
    {
        var client = _listenSessions[name].Client;
        while (true)
        {
            if (client == null)
            {
                return;
            }
            var result = await client.ReceiveAsync();
            var data = result.Buffer;
            var message = Encoding.UTF8.GetString(data);

            // Действия
            var dict = GenerateDictFromMessage(message);
            _listenResults[name].Add(new FgMessage() {Date = DateTime.Now, Values = dict});
            Console.WriteLine($"Data from FG:\n{message}");
        }
    }

    public void StopListen(string name)
    {
        if (_listenSessions.TryGetValue(name, out var info))
        {
            info.Client.Dispose();
            _listenSessions.Remove(name);
        }
    }

    public async Task<Dictionary<string, string>> GetCurrentValuesAsync(string name)
    {
        if (_listenSessions.TryGetValue(name, out var info))
        {
            var data = await info.Client.ReceiveAsync();
            var message = Encoding.UTF8.GetString(data.Buffer);
            var res = GenerateDictFromMessage(message);
            return res;
        }
        return new();
    }

    private Dictionary<string, string> GenerateDictFromMessage(string message)
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

    public void SaveResultsToFile(string filename)
    {
        // TODO 
    }
}

public class FgMessage
{
    public DateTime Date { get; init; }
    public Dictionary<string, string> Values { get; set; } = new ();
}