using System.Net;
using System.Net.Sockets;
using FlightGearApi.Domain.UtilityClasses;

namespace FlightGearApi.Domain.FlightGearCore;

public class FlightGearManipulator
{
    private UdpClient ClientSender { get; set; }
    private readonly ConnectionListener _listener;
    private readonly IoManager _ioManager;
    private readonly IPEndPoint _fgEndpoint;
    public bool ShouldFlyForward { get; set; }

    public FlightGearManipulator(ConnectionListener listener, IoManager ioManger)
    {
        _listener = listener;
        _ioManager = ioManger;
        ClientSender = new UdpClient();
        _fgEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), _ioManager.InputPort);
    }

    public async void FlyForwardCycle() // TODO: Завернуть все попытки подключения в try {} except
    {
        var setAileronToZero = false;
        while (true)
        {
            if (!ShouldFlyForward || !_listener.IsFlightGearRunning)
            {
                return;
            }
            
            var propertiesToChange = new Dictionary<UtilityProperty, double>();
            var currentProperties = await _listener.GetCurrentValuesAsync(true, UtilityProperty.Roll);
            var rollValue = currentProperties[FlightPropertiesHelper.OutputProperties[UtilityProperty.Roll].Name];
            if (setAileronToZero)
            {
                propertiesToChange[UtilityProperty.Aileron] = 0.06;
                Console.WriteLine("Set aileron to 0");
                setAileronToZero = false;
            }
            else
            {
                if (rollValue > 5 || rollValue < -5)
                {
                    setAileronToZero = true;
                    propertiesToChange[UtilityProperty.Aileron] = Math.Clamp(-rollValue/50 + 0.1, -1, 1);
                    Console.WriteLine($"Roll = {rollValue}, set aileron to {-rollValue/50}");
                }
                else
                {
                    Console.WriteLine("Roll is OK, no set aileron");
                }
                
            }
            
            await SendParametersAsync(propertiesToChange);
        }
    }
    
    public async Task SendParametersAsync(Dictionary<UtilityProperty, double> propertiesToChange)
    {
        if (!_listener.IsFlightGearRunning)
        {
            return;
        }
        var sendValues = new List<double>();
        
        var currentValues = await _listener.GetCurrentValuesAsync(true);
        
        foreach (var pairValue in FlightPropertiesHelper.InputProperties)
        {
            var curValue = currentValues[pairValue.Value.Property.Name];
            if (propertiesToChange.TryGetValue(pairValue.Key, out var newValue))
            {
                if (newValue >= pairValue.Value.MinValue && newValue <= pairValue.Value.MaxValue)
                {
                    sendValues.Add(newValue);
                }
                else if (newValue < pairValue.Value.MinValue)
                {
                    sendValues.Add(pairValue.Value.MinValue);
                }
                else
                {
                    sendValues.Add(pairValue.Value.MaxValue);
                }
            }
            else
            {
                sendValues.Add(curValue);
            }
        }
        var byteData = ConvertDoublesToBigEndianBytes(sendValues);
        
        await ClientSender.SendAsync(byteData, byteData.Length, _fgEndpoint);
    }
    
    public async Task SendParameterAsync(UtilityProperty propertyToChange, double newValue)
    {
        await SendParametersAsync(new Dictionary<UtilityProperty, double>()
        {
            {propertyToChange, newValue}
        });
    }
    
    private byte[] ConvertDoublesToBigEndianBytes(List<double> numbers)
    {
        var numberOfDoubles = numbers.Count;
        var totalBytes = sizeof(double) * numberOfDoubles;
        var data = new byte[totalBytes];

        for (var i = 0; i < numberOfDoubles; i++)
        {
            var doubleBytes = BitConverter.GetBytes(numbers[i]);
            
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(doubleBytes);
            }
            Array.Copy(doubleBytes, 0, data, i * sizeof(double), sizeof(double));
        }
        return data;
    }
}