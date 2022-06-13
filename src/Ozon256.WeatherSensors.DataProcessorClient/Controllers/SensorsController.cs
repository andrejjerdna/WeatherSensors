using Microsoft.AspNetCore.Mvc;
using Ozon256.WeatherSensors.Contracts;

namespace Ozon256.WeatherSensors.DataProcessorClient.Controllers;

[Route("subscription")]
public class SubscriptionSensorsController : Controller
{
    private readonly ISensorsDataStorage _sensorsDataStorage;

    public SubscriptionSensorsController(ISensorsDataStorage sensorsDataStorage)
    {
        _sensorsDataStorage = sensorsDataStorage;
    }
    
    [HttpGet]
    [Route(nameof(SubscriptionSensor))]
    public async Task<IActionResult> SubscriptionSensor([FromQuery]Guid guid)
    {
        var result = await _sensorsDataStorage.AddSubscriptionSensor(guid);
        
        if (!result)
            return BadRequest();
        else
            return Ok(result);
    }
    
    [HttpGet]
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