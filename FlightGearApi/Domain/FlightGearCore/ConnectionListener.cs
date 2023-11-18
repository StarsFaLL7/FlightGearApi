using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using FlightGearApi.Domain.Enums;
using FlightGearApi.Domain.Records;

namespace FlightGearApi.Domain.FlightGearCore;

public class ConnectionListener
{
    private IoManager IoManager { get; }

    private UdpClient? _client;
    private GenericConnectionInfo? _connectionInfo;

    private readonly Dictionary<string, List<FgMessage>> _listenResults = new ();

    public ConnectionListener(IoManager ioManager)
    {
        IoManager = ioManager;
    }
    
    public bool TryStartListen(string sessionName)
    {
        if (_client != null)
        {
            return false;
        }
        
        _connectionInfo = IoManager.GetConnectionInfo(IoType.Output);
        _client = new UdpClient(_connectionInfo.Port);
        _listenResults.Add(sessionName, new List<FgMessage>());
        ListenAsync(sessionName);
        return true;
    }
    
    private async void ListenAsync(string name)
    {
        while (true)
        {
            if (_client == null)
            {
                return;
            }

            try
            {
                var result = await _client.ReceiveAsync();
                var data = result.Buffer;
                var message = Encoding.UTF8.GetString(data);

                // Действия
                var dict = GenerateDictFromMessage(message);
                _listenResults[name].Add(new FgMessage() {Date = DateTime.Now, Values = dict});
            }
            catch (Exception e)
            {
                return;
            }
        }
    }

    public bool TryStopListen()
    {
        if (_client == null)
        {
            return false;
        }
        _client.Dispose();
        _client = null;
        return true;
    }

    public async Task<Dictionary<string, string>> GetCurrentValuesAsync(string name)
    {
        if (_client == null)
        {
            return new();
        }
        
        var data = await _client.ReceiveAsync();
        var message = Encoding.UTF8.GetString(data.Buffer);
        var res = GenerateDictFromMessage(message);
        return res;
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

    public void Reset()
    {
        TryStopListen();
        _listenResults.Clear();
    }

    public void SaveResultsToFile(string filename, string listenSessionName)
    {
        if (!File.Exists(filename))
        {
            File.Create(filename);
        }
        using (var f = new StreamWriter(filename))
        {
            f.Write($"Results from {_listenResults[listenSessionName][0].Date}\n");
            foreach (var msg in _listenResults[listenSessionName])
            {
                f.Write($"{msg.Date};{JsonSerializer.Serialize(msg.Values)}\n");
            }
        }
    }
}

public class FgMessage
{
    public DateTime Date { get; init; }
    public Dictionary<string, string> Values { get; init; } = new ();
}