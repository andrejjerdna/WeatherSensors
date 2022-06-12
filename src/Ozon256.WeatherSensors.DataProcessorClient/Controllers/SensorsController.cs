using Microsoft.AspNetCore.Mvc;
using Ozon256.WeatherSensors.Contracts;

namespace Ozon256.WeatherSensors.DataProcessorClient.Controllers;

[Route("sensors")]
public class SensorsController : Controller
{
    private readonly ISensorsDataStorage _sensorsDataStorage;

    public SensorsController(ISensorsDataStorage sensorsDataStorage)
    {
        _sensorsDataStorage = sensorsDataStorage;
    }
    
    [HttpGet("{guid:guid}")]
    [Route(nameof(SubscriptionSensor))]
    public async Task<IActionResult> SubscriptionSensor([FromQuery]Guid guid)
    {
        var result = await _sensorsDataStorage.AddSubscriptionSensor(guid);
        
        if (!result)
            return BadRequest();
        else
            return Ok(result);
    }
    
    [HttpGet("{guid:guid}")]
    [Route(nameof(UnsubscriptionSensor))]
    public async Task<IActionResult> UnsubscriptionSensor([FromQuery]Guid guid)
    {
        var result = await _sensorsDataStorage.AddUnsubscriptionSensor(guid);
        
        if (!result)
            return BadRequest();
        else
            return Ok(result);
    }
}