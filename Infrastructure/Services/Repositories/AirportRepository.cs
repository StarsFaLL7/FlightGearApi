using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repositories;

internal class AirportRepository : IAirportRepository
{
    private readonly PostgresDbContext _dbContext;

    public AirportRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task SaveAsync(Airport airport)
    {
        if (!_dbContext.Airports.Contains(airport))
        {
            _dbContext.Airports.Add(airport);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(Airport airport)
    {
        if (_dbContext.Airports.Contains(airport))
        {
            _dbContext.Airports.Remove(airport);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task RemoveByIdAsync(Guid airportId)
    {
        var airport = _dbContext.Airports.FirstOrDefault(a => a.Id == airportId);
        if (airport != null)
        {
            _dbContext.Airports.Remove(airport);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<Airport> GetByIdAsync(Guid id)
    {
        return _dbContext.Airports.First(a => a.Id == id);
    }

    public async Task<Airport> GetAggregateByIdAsync(Guid id)
    {
        var airport = _dbContext.Airports.Include(a => a.Runways).First(a => a.Id == id);
        return airport;
    }

    public async Task<Airport[]> GetAllAirports()
    {
        return _dbContext.Airports.ToArray();
    }
}