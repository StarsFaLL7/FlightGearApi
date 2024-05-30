using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repositories;

internal class FlightSavedSessionsRepository : IFlightSavedSessionsRepository
{
    private readonly PostgresDbContext _dbContext;

    public FlightSavedSessionsRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task SaveAsync(FlightSession session)
    {
        if (!_dbContext.FlightSessions.Contains(session))
        {
            _dbContext.FlightSessions.Add(session);
        }
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveByIdAsync(Guid sessionId)
    {
        var session = _dbContext.FlightSessions.FirstOrDefault(s => s.Id == sessionId);
        if (session != null)
        {
            _dbContext.Remove(session);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<FlightSession> GetAggregateByIdAsync(Guid sessionId)
    {
        return _dbContext.FlightSessions
            .Include(s => s.PropertiesCollection)
            .First(s => s.Id == sessionId);
    }

    public async Task<FlightSession[]> GetAllSessionsAsync()
    {
        return _dbContext.FlightSessions.ToArray();
    }
}