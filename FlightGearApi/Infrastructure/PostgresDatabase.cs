using System.Reflection;
using FlightGearApi.Application.DTO;
using FlightGearApi.Domain.Enums;
using FlightGearApi.Infrastructure.Attributes;
using FlightGearApi.Infrastructure.Interfaces;
using FlightGearApi.Infrastructure.ModelsDal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
        using (var dbContext = new PostgresDbContext(_configuration))
        {
            dbContext.FlightSessions.Add(session);
            dbContext.SaveChanges();
        }
        
        return session.Id;
    }

    public void DeleteSession(int id)
    {
        using (var dbContext = new PostgresDbContext(_configuration))
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
        using (var dbContext = new PostgresDbContext(_configuration))
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

    public FlightSessionDal? GetSessionWithProperties(int id)
    {
        using (var dbContext = new PostgresDbContext(_configuration))
        {
            var session = dbContext.FlightSessions.Include(s => s.PropertiesCollection).FirstOrDefault(s => s.Id == id);
            return session;
        }
    }
    
    public FlightSessionDal? GetSessionWithoutProperties(int id)
    {
        using (var dbContext = new PostgresDbContext(_configuration))
        {
            var session = dbContext.FlightSessions.FirstOrDefault(s => s.Id == id);
            return session;
        }
    }

    public List<FlightSessionDal> GetAllSessions()
    {
        using (var dbContext = new PostgresDbContext(_configuration))
        {
            return dbContext.FlightSessions.ToList();
        }
    }
    
    public List<FlightSessionDal> GetAllSessionsWithProperties()
    {
        using (var dbContext = new PostgresDbContext(_configuration))
        {
            return dbContext.FlightSessions.Include(s => s.PropertiesCollection).ToList();
        }
    }

    public int CreateProperties(FlightPropertiesModel properties, int sessionId)
    {
        properties.Id = sessionId;
        using (var dbContext = new PostgresDbContext(_configuration))
        {
            dbContext.FlightProperties.Add(properties);
            dbContext.SaveChanges();
        }
        return properties.Id;
    }

    public void CreatePropertiesFromRange(ICollection<FlightPropertiesModel> propertiesList, int sessionId)
    {
        using (var dbContext = new PostgresDbContext(_configuration))
        {
            foreach (var properties in propertiesList)
            {
                dbContext.FlightProperties.Add(properties);
            }
            dbContext.SaveChanges();
        }
    }

    public List<PropertiesValuesResponseDto> GetPropertiesValuesResponseList(FlightSessionDal sessionWithProperties)
    {
        var result = new List<PropertiesValuesResponseDto>();
        if (sessionWithProperties.PropertiesCollection.Count == 0)
        {
            return result;
        }
        
        var propertiesInfos = typeof(FlightPropertiesModel).GetProperties()
            .Where(p => Attribute.IsDefined(p, typeof(PropertyValueAttribute)))
            .ToArray();
        foreach (var propertyModel in sessionWithProperties.PropertiesCollection)
        {
            foreach (var property in propertiesInfos)
            {
                var attribute = property.GetCustomAttribute<PropertyValueAttribute>();
                var russianName = ExportPropertyExtensions.PropertiesInfoDict[attribute.PropertyEnum].RussianString;
                var dto = result.FirstOrDefault(m => m.Name == russianName);
                if (dto == null)
                {
                    dto = new PropertiesValuesResponseDto() { Name = russianName, Data = new List<PropertyValueDto>() };
                    result.Add(dto);
                }
                dto.Data.Add(new PropertyValueDto() { Id = propertyModel.Order, Value = (double)property.GetValue(propertyModel)});
            }
        }
        return result;
    }
}