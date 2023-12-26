using FlightGearApi.Infrastructure.ModelsDal;

namespace FlightGearApi.Infrastructure;
using Microsoft.EntityFrameworkCore;

public class PostgresDbContext : DbContext
{
    private readonly string _connectionString;
    
    public DbSet<FlightSessionDal> FlightSessions { get; set; }
    
    public DbSet<FlightPropertiesModel> FlightProperties { get; set; }

    public PostgresDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }
}