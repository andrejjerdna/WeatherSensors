using Microsoft.AspNetCore.Mvc;
using Ozon256.WeatherSensors.Contracts;

namespace Ozon256.WeatherSensors.DataProcessorClient.Controllers;

[Route("sensors")]
public class SensorsController : Controller
{
    private readonly ISensorsPool _sensorsPool;

    public SensorsController(ISensorsPool sensorsPool)
    {
        _sensorsPool = sensorsPool;
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
    public async Task<IActionResult?> DeleteSensorByGuid(Guid guid)
    {
        var sensors = await _sensorsPool.DeleteSensorsByGuid(guid);
        
        if (!sensors)
            return BadRequest();
        else
            return Ok(sensors);
    }

}