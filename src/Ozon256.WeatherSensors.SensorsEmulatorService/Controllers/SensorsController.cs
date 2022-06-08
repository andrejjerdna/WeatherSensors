using Microsoft.AspNetCore.Mvc;
using Ozon256.WeatherSensors.Contracts;

namespace Ozon256.WeatherSensors.SensorsEmulatorService.Controllers;

[Route("sensors")]
public class SensorsController : Controller
{
    private readonly ISensorsPool _sensorsPool;

    public SensorsController(ISensorsPool sensorsPool)
    {
        _sensorsPool = sensorsPool;
    }
    
    [HttpGet]
    [Route(nameof(GetAllSensorsGuids))]
    public async Task<IActionResult> GetAllSensorsGuids()
    {
        var sensors = await _sensorsPool.GetAllSensorsGuids();
        
        if (!sensors.Any())
            return NotFound();
        
        return Ok(sensors);
    }

    [HttpGet]
    [Route(nameof(GetSensorsState))]
    public async Task<IActionResult> GetSensorsState()
    {
        var sensors = await _sensorsPool.GetSensorsData();
        
        if (!sensors.Any())
            return NotFound();
        
        return Ok(sensors);
    }
    
    [HttpGet("{guid:guid}")]
    [Route(nameof(GetSensorByGuid))]
    public async Task<IActionResult> GetSensorByGuid([FromQuery]Guid guid)
    {
        var sensors = await _sensorsPool.GetSensorsByGuid(guid);
        
        if (sensors is null)
            return BadRequest();
        else
            return Ok(sensors);
    }
    
    [HttpGet("{guid:guid}")]
    [Route(nameof(DeleteSensorByGuid))]
    public async Task<IActionResult?> DeleteSensorByGuid([FromQuery]Guid guid)
    {
        var sensors = await _sensorsPool.DeleteSensorsByGuid(guid);
        
        if (!sensors)
            return BadRequest();
        else
            return Ok(sensors);
    }
    
    [HttpPost]
    [Route(nameof(AddSensor))]
    public async Task<IActionResult?> AddSensor(ISensor sensor)
    {
        var result = await _sensorsPool.AddSensors(sensor);
        
        if (!result)
            return BadRequest();
        else
            return Ok(result);
    }
}