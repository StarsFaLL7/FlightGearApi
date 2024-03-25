using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

internal class PostgresDbContext : DbContext
{
    private readonly string _connectionString;
    
    public PostgresDbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        if (_connectionString is null)
        {
            throw new Exception("Connection string 'DefaultConnection' wasn't found in appsettings.json");
        }
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }
}