using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using FlightGearApi.Application.DTO;
using FlightGearApi.Domain.Enums;
using FlightGearApi.Domain.Logging;
using FlightGearApi.Domain.Records;
using FlightGearApi.Domain.UtilityClasses;

namespace FlightGearApi.Domain.FlightGearCore;

public class FlightGearManipulator
{
    private const int AltitudeError = 30;
    private const int SpeedError = 5;
    private const int HeadingError = 15;

    
    private readonly UdpClient _clientSender;
    private readonly ConnectionListener _listener;
    private readonly FlightGearLauncher _launcher;
    private readonly IPEndPoint _fgEndpoint;
    
    private bool _islLowSpeed;
    private int _currentStageIndex;
    
    public bool ShouldFlyForward { get; set; }
    public List<FlightStageModel> Stages { get; }
    
    
    public FlightGearManipulator(ConnectionListener listener, IoManager ioManger, FlightGearLauncher launcher)
    {
        _listener = listener;
        _launcher = launcher;
        _clientSender = new UdpClient();
        _fgEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), ioManger.InputPort);
        Stages = new List<FlightStageModel>();
    }

    public void AddStage(FlightStageModel stage)
    {
        if (Stages.Count == 0)
        {
            Stages.Add(stage);
            stage.Index = 0;
            return;
        }
        if (stage.Index > Stages.Count)
        {
            stage.Index = Stages.Count;
        }
        Stages.Insert(stage.Index, stage);
        
        
        for (var i = stage.Index + 1; i < Stages.Count-1; i++)
        {
            Stages[i].Index++;
        }
    }

    public async void FlyCycle()
    {
        await StaticLogger.LogAsync(LogLevel.Information, $"Fly Cycle Started in {this.GetType()}");
        
        var initialProperties = new Dictionary<UtilityProperty, double>()
        {
            { UtilityProperty.ApPitchSwitch, 1},
            { UtilityProperty.ApRollSwitch, 1},
            { UtilityProperty.ApHeadingSwitch, 1},
            { UtilityProperty.Flaps, 0}
        };
        await SendParametersAsync(initialProperties);
        
        while (true)
        {
            try
            {
                if (!_listener.IsFlightGearRunning || !ShouldFlyForward)
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
                
                var altitudeFt = currentProperties[UtilityProperty.Altitude.GetName()];
                var headingValue = currentProperties[UtilityProperty.Heading.GetName()];
                var indicatedSpeed = currentProperties[UtilityProperty.IndicatedSpeed.GetName()];
                
                _islLowSpeed = indicatedSpeed < 80;
                
                var verticalRate = GetTargetVerticalPressureRate(altitudeFt, indicatedSpeed);
                propertiesToChange[UtilityProperty.ApTargetVerticalPressureRate] = verticalRate;
                if (_islLowSpeed)
                {
                    Console.WriteLine("Emergency! LOW SPEED!");
                    propertiesToChange[UtilityProperty.ApHeadingSwitch] = 0;
                }
                else
                {
                    propertiesToChange[UtilityProperty.ApHeadingSwitch] = 1;
                }

                var headingBug = Stages[_currentStageIndex].Heading;
                propertiesToChange[UtilityProperty.ApHeadingHeadingDeg] = headingBug;

                var throttle = GetThrottleValue(indicatedSpeed, verticalRate);
                propertiesToChange[UtilityProperty.Throttle] = throttle;

                await SendParametersAsync(propertiesToChange);
                Console.WriteLine($"Current values:\n\talt: {altitudeFt}\n\theading: {headingValue}\n\tspeed: {indicatedSpeed}");
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

    private double GetTargetVerticalPressureRate(double altitude, double indicatedSpeed) // TODO Поработать с множителями, медленно набирает/снижается
    {
        var goalAltitude = Stages[_currentStageIndex].Altitude;
        var goalVerticalSpeed = 0d;
        if (Math.Abs(altitude - goalAltitude) < AltitudeError)
        {
            Console.WriteLine("ALT: Goal altitude achieved!");
            goalVerticalSpeed = 0;
        }
        else
        {
            Console.WriteLine("ALT: Goal altitude != altitude");
            goalVerticalSpeed = Math.Clamp(goalAltitude - altitude, -1000, 1000);
            if (_islLowSpeed && goalVerticalSpeed > 0)
            {
                goalVerticalSpeed = -(1000 - indicatedSpeed/80*500);
            }
        }
        var result = goalVerticalSpeed / -58000;
        Console.WriteLine($"ALT: set to {result}");
        return result;
    }

    private double GetThrottleValue(double indicatedSpeed, double targetVerticalRate) // TODO Поработать с величинами throttle, при 0.6 (default) падает скорость
    {
        var goalSpeed = Stages[_currentStageIndex].Speed;
        if (_islLowSpeed)
        {
            return 1;
        }
        if (targetVerticalRate == 0)
        {
            if (Math.Abs(goalSpeed - indicatedSpeed) < SpeedError)
            {
                return 0.8;
            }
            if (goalSpeed < indicatedSpeed)
            {
                return 0.6;
            }
            return 1;
        }
        if (targetVerticalRate < 0)
        {
            return 1;
        }

        return 0.6;
    }

    public void CheckIsGoalAchieved(double heading, double speed, double altitude)
    {
        var currentStepGoal = Stages[_currentStageIndex];
        if (Math.Abs(heading - currentStepGoal.Heading) < HeadingError &&
            Math.Abs(speed - currentStepGoal.Speed) < SpeedError &&
            Math.Abs(altitude - currentStepGoal.Altitude) < AltitudeError)
        {
            if (Stages.Count > _currentStageIndex + 1)
            {
                _currentStageIndex++;
                StaticLogger.Log(LogLevel.Information, $"Simulation stage {_currentStageIndex-1} completed, left: {Stages.Count - _currentStageIndex}");
            }
            else
            {
                _launcher.Exit();
                Console.WriteLine("--- Mission completed! ---");
                StaticLogger.Log(LogLevel.Information, "All simulation stages successfully completed!");
                // TODO SAVE RESULTS TO BD
            }
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
        
        await _clientSender.SendAsync(byteData, byteData.Length, _fgEndpoint);
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