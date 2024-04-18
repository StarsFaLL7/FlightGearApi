using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

internal class PostgresDbContext : DbContext
{
    private readonly string _connectionString;

    public DbSet<Airport> Airports { get; set; }
    public DbSet<AirportRunway> AirportRunways { get; set; }
    public DbSet<FlightPlan> FlightPlans { get; set; }
    public DbSet<FlightPropertiesShot> FlightPropertiesShots { get; set; }
    public DbSet<FlightSession> FlightSessions { get; set; }
    public DbSet<FunctionPoint> FunctionPoints { get; set; }
    public DbSet<ReadyFlightFunction> FlightFunctions { get; set; }
    public DbSet<RoutePoint> RoutePoints { get; set; }
    
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