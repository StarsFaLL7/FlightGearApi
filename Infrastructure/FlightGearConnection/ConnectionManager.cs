using System.Globalization;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Application.Interfaces.Connection;
using Domain.Attributes;
using Domain.Entities;
using Domain.Enums.FlightExportProperty;
using Domain.Enums.FlightUtilityProperty;
using Domain.Utility;
using Microsoft.Extensions.Configuration;
using Exception = System.Exception;

namespace Infrastructure.FlightGearConnection;

/// <inheritdoc />
internal class ConnectionManager : IConnectionManager
{
    private readonly int _telnetPort;

    private TcpClient? _tcpClient;
    private NetworkStream? _networkTcpStream;
    
    private SemaphoreSlim _tcpClientSemaphore = new SemaphoreSlim(1, 1);
    public ConnectionManager(IConfiguration configuration)
    {
        var telnetPort = configuration.GetSection("FlightGearSettings").GetSection("Connections")
            .GetSection("TelnetPort").Value;
        if (string.IsNullOrWhiteSpace(telnetPort))
        {
            throw new Exception("Incorrect value for telnet port in appsettings.json " +
                                "\"FlightGearSettings/Connections/TelnetPort\"");
        }

        _telnetPort = int.Parse(telnetPort);
        
    }

    public async Task<Dictionary<string, double>> GetCurrentUtilityValuesAsync(params FlightUtilityProperty[] properties)
    {
        var result = new Dictionary<string, double>();

        if (properties.Length == 0)
        {
            return result;
        }

        var infos = properties.Select(p => p.GetInfo()).ToArray();
        
        var commands = infos.Select(p => $"get {p.Path}").ToArray();
        var responses = SendCommands(commands, true);
        var i = 0;
        foreach (var response in responses)
        {
            var propertyInfo = infos[i];
            var value = ParseDoubleFromResponse(response.Response) * propertyInfo.Multiplier;
            result[propertyInfo.Name] = value;
            i++;
        }
        
        return result;
    }

    public async Task<FlightPropertiesShot> GetCurrentValuesAsync()
    {
        var result = new FlightPropertiesShot
        {
            Order = 0,
            FlightSessionId = default,
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

        var propertiesDict = new Dictionary<FlightExportProperty, PropertyInfo>();

        foreach (var propertyInfo in typeof(FlightPropertiesShot).GetProperties())
        {
            var attribute = propertyInfo.GetCustomAttribute<FlightPropertyInfoAttribute>();
            if (attribute == null)
            {
                continue;
            }

            propertiesDict[attribute.ExportPropertyEnum] = propertyInfo;
        }

        var commands = FlightExportPropertyExtensions.PropertiesInfoDict.Select(pair => $"get {pair.Value.Path}").ToArray();
        var responses = SendCommands(commands, true);

        var i = 0;
        foreach (var keyValuePair in FlightExportPropertyExtensions.PropertiesInfoDict)
        {
            var response = responses[i];
            var value = ParseDoubleFromResponse(response.Response) * keyValuePair.Value.Multiplier;
            propertiesDict[keyValuePair.Key].SetValue(result, value);
            i++;
        }
        
        return result;
    }

    public async Task<double> GetPropertyDoubleValueAsync(string propertyPath)
    {
        if (string.IsNullOrWhiteSpace(propertyPath))
        {
            return 0;
        }
        var response = SendCommand($"get {propertyPath}", true);
        var result = ParseDoubleFromResponse(response);
        return result;
    }

    public async Task<string?> GetPropertyStringValueAsync(string propertyPath)
    {
        if (string.IsNullOrWhiteSpace(propertyPath))
        {
            return "";
        }
        var response = SendCommand($"get {propertyPath}", true);
        return ParseStringFromResponse(response);
    }

    public async Task SetParametersAsync(Dictionary<FlightUtilityProperty, double> propertiesToChange)
    {
        if (propertiesToChange.Count == 0)
        {
            return;
        }
        
        var commands = propertiesToChange.Select(pair =>
        {
            var propInfo = pair.Key.GetInfo();
            return $"set {propInfo.Path} {pair.Value}";
        }).ToArray();
        SendCommands(commands, false);
    }

    public async Task SetPropertyAsync(string propertyPath, object value)
    {
        if (string.IsNullOrWhiteSpace(propertyPath))
        {
            return;
        }

        SendCommand($"set {propertyPath} {value}", true);
    }

    public string? SendCommand(string command, bool readLine)
    {
        CreateTcpClientIfNotExist();
        string? response = null;
        _tcpClientSemaphore.Wait();
        try
        {
            using var writer = new StreamWriter(_networkTcpStream, Encoding.ASCII, leaveOpen: true);
            if (readLine)
            {
                using var reader = new StreamReader(_networkTcpStream, Encoding.ASCII, leaveOpen: true);
                writer.WriteLine(command);
                writer.Flush();
                response = reader.ReadLine();
            }
            else
            {
                writer.WriteLine(command);
                writer.Flush();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while trying to send command {command}, readLines={readLine} by Telnet in ConnectionListener {e}");
        }
        finally
        {
            _tcpClientSemaphore.Release();
        }

        return response;
    }
    
    private List<(string Command, string? Response)> SendCommands(string[] commands, bool readLines)
    {
        CreateTcpClientIfNotExist();
        var result = new List<(string Command, string? Response)>();
        _tcpClientSemaphore.Wait();
        try
        {
            using var writer = new StreamWriter(_networkTcpStream, Encoding.ASCII, leaveOpen: true);
            using var reader = new StreamReader(_networkTcpStream, Encoding.ASCII, leaveOpen: true);
            foreach (var command in commands)
            {
                writer.WriteLine(command);
                writer.Flush();
                if (readLines)
                {
                    var response = reader.ReadLine();
                    result.Add((command, response));
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while trying to send commands {commands}, readLines={readLines} by Telnet in ConnectionListener {e}");
        }
        finally
        {
            _tcpClientSemaphore.Release();
        }

        return result;
    }
    
    private void CreateTcpClientIfNotExist()
    {
        if (_tcpClient is null || _networkTcpStream is null)
        {
            _tcpClient = new TcpClient("127.0.0.1", _telnetPort);
            _networkTcpStream = _tcpClient.GetStream();
        }

        if (!_tcpClient.Connected)
        {
            _tcpClient = new TcpClient("127.0.0.1", _telnetPort);
            _networkTcpStream = _tcpClient.GetStream();
            throw new Exception($"Tcp client couldn't connect to 127.0.0.1:{_telnetPort}");
        }
        if (!_tcpClient.Connected)
        {
            throw new Exception($"Tcp client couldn't connect to 127.0.0.1:{_telnetPort}");
        }
    }
    
    private double ParseDoubleFromResponse(string? response)
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
    
    private string? ParseStringFromResponse(string? response)
    {
        if (string.IsNullOrWhiteSpace(response))
        {
            throw new ArgumentException("Invalid response provided.");
        }

        var commaIndex = response.IndexOf('\'')+1;

        var valueString =
            response.Substring(commaIndex, response.LastIndexOf('\'') - commaIndex);

        return valueString;
    }
    
}