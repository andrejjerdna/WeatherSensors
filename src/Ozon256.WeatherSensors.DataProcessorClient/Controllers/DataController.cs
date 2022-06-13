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
    
    [HttpGet]
    [Route(nameof(SensorDataSelectionByInterval))]
    public async Task<IActionResult> SensorDataSelectionByInterval([FromQuery]Guid guid, [FromQuery]DateTime startTime, [FromQuery]DateTime endTime)
    {
        var result = await _dataManager.GetSensorDataByGuidForInterval(guid, startTime, endTime);
        
        if (result is null)
            return BadRequest();
        else
            return Ok(result);
    }
    
    [HttpGet]
    [Route(nameof(SensorDataSelectionByMinutes))]
    public async Task<IActionResult> SensorDataSelectionByMinutes([FromQuery]Guid guid, [FromQuery]DateTime startTime, [FromQuery]int minutes)
    {
        var result = await _dataManager.GetSensorDataByGuidForInterval(guid, startTime, minutes);
        
        if (result is null)
            return BadRequest();
        else
            return Ok(result);
    }
}