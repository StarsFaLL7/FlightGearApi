using System.Data;
using FlightGearApi.Infrastructure.Interfaces;
using FlightGearApi.Infrastructure.ModelsDal;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FlightGearApi.Infrastructure;

public class PostgresDatabase : IPostgresDatabase
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public PostgresDatabase(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public int CreateSession(FlightSessionDal session)
    {
        session.Date = TimeZoneInfo.ConvertTimeToUtc(session.Date);
        using (var dbContext = new PostgresDbContext(_connectionString))
        {
            dbContext.FlightSessions.Add(session);
            dbContext.SaveChanges();
        }
        
        return session.Id;
    }

    public void DeleteSession(int id)
    {
        using (var dbContext = new PostgresDbContext(_connectionString))
        {
            var session = dbContext.FlightSessions.FirstOrDefault(s => s.Id == id);
            if (session != null)
            {
                dbContext.FlightSessions.Remove(session);
                dbContext.SaveChanges();
            }
        }
    }

    public void UpdateSession(FlightSessionDal session)
    {
        using (var dbContext = new PostgresDbContext(_connectionString))
        {
            var sessionToUpdate = dbContext.FlightSessions.FirstOrDefault(s => s.Id == session.Id);
            if (sessionToUpdate != null)
            {
                sessionToUpdate.Date = session.Date;
                sessionToUpdate.Title = session.Title;
                sessionToUpdate.DurationSec = session.DurationSec;
                dbContext.SaveChanges();
            }
        }
    }

    public FlightSessionDal? GetSession(int id)
    {
        using (var dbContext = new PostgresDbContext(_connectionString))
        {
            var session = dbContext.FlightSessions.FirstOrDefault(s => s.Id == id);
            return session;
        }
    }

    public List<FlightSessionDal> GetAllSessions()
    {
        using (var dbContext = new PostgresDbContext(_connectionString))
        {
            return dbContext.FlightSessions.Include(s => s.PropertiesCollection).ToList();
        }
    }

    public int CreateProperties(FlightPropertiesModel properties, int sessionId)
    {
        properties.Id = sessionId;
        using (var dbContext = new PostgresDbContext(_connectionString))
        {
            dbContext.FlightProperties.Add(properties);
            dbContext.SaveChanges();
        }
        return properties.Id;
    }

    public void CreatePropertiesFromRange(ICollection<FlightPropertiesModel> propertiesList, int sessionId)
    {
        using (var dbContext = new PostgresDbContext(_connectionString))
        {
            foreach (var properties in propertiesList)
            {
                dbContext.FlightProperties.Add(properties);
            }
            dbContext.SaveChanges();
        }
    }
}