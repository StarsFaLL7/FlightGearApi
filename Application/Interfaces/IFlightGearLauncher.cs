﻿namespace Application.Interfaces;

public interface IFlightGearLauncher
{
    Task<bool> TryLaunchSimulationAsync();
    
    
}