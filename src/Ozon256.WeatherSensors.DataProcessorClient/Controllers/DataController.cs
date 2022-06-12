using Microsoft.AspNetCore.Mvc;
using Ozon256.WeatherSensors.Contracts;

namespace Ozon256.WeatherSensors.DataProcessorClient.Controllers;

[Route("data")]
public class DataController : Controller
{
    private readonly IDataManager _dataManager;

    public DataController(IDataManager dataManager)
    {
        _dataManager = dataManager;
    }
    
    [HttpGet("{guid:guid}, {startTime:datetime}, {endTime:datetime}")]
    [Route(nameof(SensorDataSelectionByInterval))]
    public async Task<IActionResult> SensorDataSelectionByInterval([FromQuery]Guid guid, DateTime startTime, DateTime endTime)
    {
        var result = await _dataManager.GetSensorDataByGuidForInterval(guid, startTime, endTime);
        
        if (result is null)
            return BadRequest();
        else
            return Ok(result);
    }
    
    [HttpGet("{guid:guid}, {startTime:datetime}, {endTime:datetime}")]
    [Route(nameof(SensorDataSelectionByMinutes))]
    public async Task<IActionResult> SensorDataSelectionByMinutes([FromQuery]Guid guid, DateTime startTime, int minutes)
    {
        var result = await _dataManager.GetSensorDataByGuidForInterval(guid, startTime, minutes);
        
        if (result is null)
            return BadRequest();
        else
            return Ok(result);
    }
}