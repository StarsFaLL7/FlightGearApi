﻿using FlightGearApi.Application.DTO;
using FlightGearApi.Infrastructure.ModelsDal;

namespace FlightGearApi.Infrastructure.Interfaces;

public interface IPostgresDatabase
{
    int CreateSession(FlightSessionDal session);
    
    void DeleteSession(int id);
    
    void UpdateSession(FlightSessionDal user);
    
    FlightSessionDal? GetSessionWithProperties(int id);
    
    FlightSessionDal? GetSessionWithoutProperties(int id);
    
    List<FlightSessionDal> GetAllSessions();

    int CreateProperties(FlightPropertiesModel properties, int sessionId);
    
    void CreatePropertiesFromRange(ICollection<FlightPropertiesModel> propertiesList, int sessionId);

    public List<PropertiesValuesResponseDto> GetPropertiesValuesResponseList(FlightSessionDal sessionWithProperties);
}