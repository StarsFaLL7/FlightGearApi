﻿using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repositories;

internal class FlightPlanRepository : IFlightPlanRepository
{
    private readonly PostgresDbContext _dbContext;

    public FlightPlanRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task SaveAsync(FlightPlan plan)
    {
        if (!_dbContext.FlightPlans.Contains(plan))
        {
            _dbContext.FlightPlans.Add(plan);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveByIdAsync(Guid planId)
    {
        var plan = _dbContext.FlightPlans.FirstOrDefault(p => p.Id == planId);
        if (plan != null)
        {
            _dbContext.Remove(plan);
        }
        await _dbContext.SaveChangesAsync();
    }

    public async Task<FlightPlan> GetByIdAsync(Guid id)
    {
        return _dbContext.FlightPlans.First(p => p.Id == id);
    }

    public async Task<FlightPlan> GetAggregateByIdAsync(Guid id)
    {
        return _dbContext.FlightPlans
            .Include(p => p.ArrivalRunway)
            .Include(p => p.DepartureRunway)
            .Include(p => p.RoutePoints)
            .First(p => p.Id == id);
    }
}