using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

internal class PostgresDbContext : DbContext
{
    private readonly string _connectionString;

    public DbSet<Airport> Airports;
    public DbSet<AirportRunway> AirportRunways;
    public DbSet<FlightPlan> FlightPlans;
    public DbSet<FlightPropertiesShot> FlightPropertiesShots;
    public DbSet<FlightSession> FlightSessions;
    public DbSet<FunctionPoint> FunctionPoints;
    public DbSet<ReadyFlightFunction> FlightFunctions;
    public DbSet<RoutePoint> RoutePoints;
    
    public PostgresDbContext(IConfiguration configuration)
    {
        var readedConnString = configuration.GetConnectionString("DefaultConnection");
        if (readedConnString is null)
        {
            throw new Exception("Connection string 'DefaultConnection' wasn't found in appsettings.json");
        }
        _connectionString = readedConnString;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }
}