using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repositories;

internal class ReadyFlightFunctionRepository : IReadyFlightFunctionRepository
{
    private readonly PostgresDbContext _dbContext;

    public ReadyFlightFunctionRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task SaveAsync(ReadyFlightFunction function)
    {
        if (!_dbContext.FlightFunctions.Contains(function))
        {
            _dbContext.FlightFunctions.Add(function);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveByIdAsync(Guid functionId)
    {
        var function = _dbContext.FlightFunctions.FirstOrDefault(f => f.Id == functionId);
        if (function != null)
        {
            _dbContext.FlightFunctions.Remove(function);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<ReadyFlightFunction> GetByIdAsync(Guid id)
    {
        return _dbContext.FlightFunctions.First(f => f.Id == id);
    }

    public async Task<ReadyFlightFunction> GetAggregateByIdAsync(Guid id)
    {
        return _dbContext.FlightFunctions
            .Include(f => f.FunctionPoints)
            .First(f => f.Id == id);
    }

    public async Task<ReadyFlightFunction[]> GetAllAsync()
    {
        return _dbContext.FlightFunctions.Include(f => f.FunctionPoints).ToArray();
    }
}