using System.Globalization;
using System.Net;
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

        CreateTcpClientIfNotExist();

        await _tcpClientSemaphore.WaitAsync();
        try
        {
            await using var writer = new StreamWriter(_networkTcpStream, Encoding.ASCII, leaveOpen: true);
            using var reader = new StreamReader(_networkTcpStream, Encoding.ASCII, leaveOpen: true);

            foreach (var property in properties)
            {
                var propInfo = property.GetInfo();
                await writer.WriteLineAsync($"get {propInfo.Path}");
                await writer.FlushAsync();
                var response = await reader.ReadLineAsync();
                var value = ParseDoubleFromResponse(response) * propInfo.Multiplier;
                result[propInfo.Name] = value;
            }
        }
        catch (Exception e)
        {
            // Todo: logging
            Console.WriteLine($"Error while trying to get currentValues by Telnet in ConnectionListener {e}");
        }
        finally
        {
            _tcpClientSemaphore.Release();
        }
        return result;
    }

    public async Task<FlightPropertiesShot> GetCurrentValuesAsync()
    {
        var result = new FlightPropertiesShot
        {
            Order = 0,
            FlightSessionId = 0,
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
        
        CreateTcpClientIfNotExist();
        await _tcpClientSemaphore.WaitAsync();

        try
        {
            await using var writer = new StreamWriter(_networkTcpStream, Encoding.ASCII, leaveOpen: true);
            using var reader = new StreamReader(_networkTcpStream, Encoding.ASCII, leaveOpen: true);

            foreach (var keyValuePair in FlightExportPropertyExtensions.PropertiesInfoDict)
            {
                var path = keyValuePair.Value.Path;
                await writer.WriteLineAsync($"get {path}");
                await writer.FlushAsync();
                var response = await reader.ReadLineAsync();
                var value = ParseDoubleFromResponse(response) * keyValuePair.Value.Multiplier;
                propertiesDict[keyValuePair.Key].SetValue(result, value);
            }
        }
        catch (Exception e)
        {
            // Todo: logging
            Console.WriteLine($"Error while trying to get currentValues by Telnet in ConnectionListener {e}");
        }
        finally
        {
            _tcpClientSemaphore.Release();
        }
        return result;
    }
    
    public async Task SendParametersAsync(Dictionary<FlightUtilityProperty, double> propertiesToChange)
    {
        if (propertiesToChange.Count == 0)
        {
            return;
        }
        CreateTcpClientIfNotExist();

        await _tcpClientSemaphore.WaitAsync();
        try
        {
            await using var writer = new StreamWriter(_networkTcpStream, Encoding.ASCII, leaveOpen: true);

            foreach (var property in propertiesToChange)
            {
                var propInfo = property.Key.GetInfo();
                await writer.WriteLineAsync($"set {propInfo.Path} {property.Value}");
                await writer.FlushAsync();
            }
        }
        catch (Exception e)
        {
            // Todo: logging
            Console.WriteLine($"Error while trying to set values by Telnet in ConnectionListener {e}");
        }
        finally
        {
            _tcpClientSemaphore.Release();
        }
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
}