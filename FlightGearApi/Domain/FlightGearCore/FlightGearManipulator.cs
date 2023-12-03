using System.Net;
using System.Net.Sockets;
using FlightGearApi.Domain.Enums;
using FlightGearApi.Domain.Records;
using FlightGearApi.Domain.UtilityClasses;

namespace FlightGearApi.Domain.FlightGearCore;

public class FlightGearManipulator
{
    private UdpClient ClientSender { get; set; }
    private readonly ConnectionListener _listener;
    private readonly FlightGearLauncher _launcher;
    private readonly IPEndPoint _fgEndpoint;
    public bool ShouldFlyForward { get; set; }

    private bool _setAileronToZero;
    private bool _setElevatorToZero;

    public List<FlightStepGoal> StepsToAchieve = new List<FlightStepGoal>()
    {
        new FlightStepGoal(1000, 180, 54),
        new FlightStepGoal(1500, 90, 54)
    };

    public int CurrentStepIndex;
    
    public FlightGearManipulator(ConnectionListener listener, IoManager ioManger, FlightGearLauncher launcher)
    {
        _listener = listener;
        _launcher = launcher;
        ClientSender = new UdpClient();
        _fgEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), ioManger.InputPort);
    }

    public async void FlyCycle() // TODO: Завернуть все попытки подключения в try {} except
    {
        while (true)
        {
            try
            {
                if (!_listener.IsFlightGearRunning)
                {
                    return;
                }

                var propertiesToChange = new Dictionary<UtilityProperty, double>();
                var currentProperties = await _listener.GetCurrentValuesAsync(true,
                    UtilityProperty.Roll, UtilityProperty.Altitude, UtilityProperty.Elevator,
                    UtilityProperty.VerticalSpeed, UtilityProperty.Throttle, UtilityProperty.Heading,
                    UtilityProperty.IndicatedSpeed, UtilityProperty.Rudder);

                CheckIsGoalAchieved(currentProperties[UtilityProperty.Heading.GetName()],
                    currentProperties[UtilityProperty.IndicatedSpeed.GetName()],
                    currentProperties[UtilityProperty.Altitude.GetName()]);

                var rollValue = currentProperties[UtilityProperty.Roll.GetName()];
                if (IsAileronShouldBeChanged(rollValue, out var aileronValue))
                {
                    propertiesToChange[UtilityProperty.Aileron] = aileronValue;
                }

                var altitudeFt = currentProperties[UtilityProperty.Altitude.GetName()];
                var currElevatorValue = currentProperties[UtilityProperty.Elevator.GetName()];
                var verticalSpeedValue = currentProperties[UtilityProperty.VerticalSpeed.GetName()];
                var throttleValue = currentProperties[UtilityProperty.Throttle.GetName()];
                if (IsElevatorShouldBeChanged(altitudeFt, StepsToAchieve[CurrentStepIndex].Altitude, currElevatorValue,
                        verticalSpeedValue,
                        throttleValue, rollValue, out var newElevatorValue, out var newThrottleValue))
                {
                    propertiesToChange[UtilityProperty.Elevator] = newElevatorValue;
                    propertiesToChange[UtilityProperty.Throttle] = newThrottleValue;
                }

                var headingValue = currentProperties[UtilityProperty.Heading.GetName()];
                var rudderValue = currentProperties[UtilityProperty.Rudder.GetName()];
                if (IsRudderShouldBeChanged(headingValue, StepsToAchieve[CurrentStepIndex].Heading, rudderValue,
                        out var newRudderValue))
                {
                    propertiesToChange[UtilityProperty.Rudder] = newRudderValue;
                }

                await SendParametersAsync(propertiesToChange);
                Console.WriteLine("---------------");
            }
            catch (SocketException se)
            {
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    public void CheckIsGoalAchieved(double heading, double speed, double altitude)
    {
        var currentStepGoal = StepsToAchieve[CurrentStepIndex];
        if (Math.Abs(heading - currentStepGoal.Heading) < 5 &&
            Math.Abs(speed - currentStepGoal.Speed) < 5 &&
            Math.Abs(altitude - currentStepGoal.Altitude) < 5)
        {
            if (StepsToAchieve.Count > CurrentStepIndex + 1)
            {
                CurrentStepIndex++;
            }
            else
            {
                _launcher.Exit();
                Console.WriteLine("--- Mission completed! ---");
                // TODO SAVE RESULTS TO BD
            }
        }
    }
    
    public bool IsAileronShouldBeChanged(double rollValue, out double aileronValue) 
    {
        if (_setAileronToZero)
        {
            Console.WriteLine("Aileron: set 0");
            _setAileronToZero = false;
            aileronValue = 0.06;
            return true;
        }
        if (rollValue > 1 || rollValue < -1)
        {
            _setAileronToZero = true;
            Console.WriteLine($"Aileron: Roll = {rollValue}, set aileron to {Math.Clamp(-rollValue/50 + 0.1, -0.5, 0.5)}");
            aileronValue = Math.Clamp(-rollValue/50 + 0.1, -0.5, 0.5);
            return true;
        }
        Console.WriteLine("Aileron: Roll is OK");
        aileronValue = -1;
        return false;
    }
    
    public bool IsElevatorShouldBeChanged(double altitude, double goalAltitude, double currElevatorValue, 
        double verticalSpeed, double throttleValue, double rollValue, out double newElevatorValue, out double newThrottleValue)
    {
        if (_setElevatorToZero || (rollValue > 10 || rollValue < -10))
        {
            newElevatorValue = 0;
            newThrottleValue = throttleValue;
            _setElevatorToZero = false;
            return true;
        }
        
        if (Math.Abs(goalAltitude - altitude) < 50)
        {
            _setElevatorToZero = true;
        }

        if (goalAltitude > altitude && verticalSpeed < 5)
        {
            Console.WriteLine("Elevator: Trying to go upper");
            newElevatorValue = -0.1;
            newThrottleValue = throttleValue;
            _setElevatorToZero = true;
            return true;
        }

        if (goalAltitude < altitude && verticalSpeed > -5)
        {
            Console.WriteLine("Elevator: Trying to go down");
            newElevatorValue = 0.1;
            _setElevatorToZero = true;
            newThrottleValue = Math.Clamp(throttleValue+0.1, 0, 1);
            return true;
        }
        Console.WriteLine("Elevator: In progress to goal");
        newElevatorValue = currElevatorValue;
        newThrottleValue = throttleValue;
        return false;
    }

    public bool IsRudderShouldBeChanged(double currentHeading, double goalHeading, double rudderValue, out double newRudderValue)
    {
        if (Math.Abs(currentHeading - goalHeading) < 5)
        {
            Console.WriteLine("Rudder: goal achieved");
            newRudderValue = 0;
            return true;
        }

        if (currentHeading < goalHeading && 360 - goalHeading + currentHeading < goalHeading - currentHeading || 
            currentHeading > goalHeading && currentHeading - goalHeading < 36 - currentHeading + goalHeading)
        {
            Console.WriteLine("Rudder: set to -0.1 (left)");
            newRudderValue = -0.1;
            return true;
        }
        Console.WriteLine("Rudder: set to 0.1 (right)");
        newRudderValue = 0.1;
        return true;
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