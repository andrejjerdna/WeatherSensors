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
 }