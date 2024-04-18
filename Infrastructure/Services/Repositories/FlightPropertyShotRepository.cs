using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Services.Repositories;

internal class FlightPropertyShotRepository : IFlightPropertiesShotRepository
{
    private readonly PostgresDbContext _dbContext;

    public FlightPropertyShotRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveRangeAsync(params FlightPropertiesShot[] shots)
    {
        await _dbContext.FlightPropertiesShots.AddRangeAsync(shots);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<FlightPropertiesShot[]> GetAllBySessionIdAsync(Guid sessionId)
    {
        return _dbContext.FlightPropertiesShots
            .Where(p => p.FlightSessionId == sessionId)
            .OrderBy(p => p.Order)
            .ToArray();
    }
}