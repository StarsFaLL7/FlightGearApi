using System.Reflection;
using FlightGearApi.Application.DTO;
using FlightGearApi.Domain.Enums;
using FlightGearApi.Domain.FlightGearCore;
using FlightGearApi.Infrastructure.Attributes;
using FlightGearApi.Infrastructure.Interfaces;
using FlightGearApi.Infrastructure.ModelsDal;
using Microsoft.AspNetCore.Mvc;

namespace FlightGearApi.Application.Controllers;

[Route("api/analytics")]
public class AnalyticsController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IoManager _ioManager;
    private readonly FlightGearLauncher _launcher;
    private readonly ConnectionListener _listener;
    private readonly FlightGearManipulator _manipulator;
    private readonly ExportParametersManager _exportManager;
    private readonly IPostgresDatabase _database;
    
    public AnalyticsController(IConfiguration configuration, IoManager ioManager, FlightGearLauncher launcher, 
        ConnectionListener listener, FlightGearManipulator manipulator, ExportParametersManager exportManager,
        IPostgresDatabase database)
    {
        _configuration = configuration;
        _ioManager = ioManager;
        _launcher = launcher;
        _listener = listener;
        _manipulator = manipulator;
        _exportManager = exportManager;
        _database = database;
    }
    
    /// <summary>
    /// Получить список всех сессий, сохранённых в БД, без списка параметров (null)
    /// </summary>
    /// <returns></returns>
    [HttpGet("sessions")]
    [ProducesResponseType(typeof(List<FlightSessionDal>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSessions()
    {
        return Ok(_database.GetAllSessions());
    }
    
    /// <summary>
    /// Получить список параметров и кол-во их считываний для сессии с id = sessionId
    /// </summary>
    /// <param name="sessionId">Id сессии, параметры которой нужно получить</param>
    /// <returns></returns>
    [HttpGet("sessions/{sessionId:int}/properties-count")]
    [ProducesResponseType(typeof(IEnumerable<FlightPropertyInfoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPropertiesList(int sessionId)
    {
        var session = _database.GetSessionWithProperties(sessionId);
        if (session == null)
        {
            return NotFound("No session with given id found.");
        }
        var result = ExportPropertyExtensions.PropertiesInfoDict.Select(p => new FlightPropertyInfoDto()
            { Name = p.Value.RussianString, Count = session.PropertiesCollection.Count });
        return Ok(result);
    }
    
    /// <summary>
    /// Получить значения считываний параметров для сессии с id = sessionId
    /// </summary>
    /// <param name="sessionId">Id сессии, значения параметров которой нужно получить</param>
    /// <returns></returns>
    [HttpGet("sessions/{sessionId:int}/properties-values")]
    [ProducesResponseType(typeof(List<PropertiesValuesResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPropertiesValues(int sessionId)
    {
        var session = _database.GetSessionWithProperties(sessionId);
        if (session == null)
        {
            return NotFound("No session with given id found.");
        }

        var result = new List<PropertiesValuesResponseDto>();
        if (session.PropertiesCollection.Count == 0)
        {
            return Ok(result);
        }
        var propertiesInfos = new FlightPropertiesModel().GetType().GetProperties()
            .Where(p => Attribute.IsDefined(p, typeof(PropertyValueAttribute)))
            .ToArray();
        foreach (var propertyModel in session.PropertiesCollection)
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
        return Ok(result);
    }
}