using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Options;
using Ozon256.WeatherSensors.Contracts;
using Ozon256.WeatherSensors.SensorsEmulatorService;
using Ozon256.WeatherSensors.SensorsEmulatorService.Options;

namespace Ozon256.WeatherSensors.SensorsEmulatorService.Services;

public class SensorsService : Sensors.SensorsBase
{
    private readonly ISensorsPool _sensorsPool;
    private readonly IOptions<SensorsPoolConfig> _sensorsPoolConfig;
    private readonly IServiceProvider _provider;
    private readonly ILogger<SensorsService> _logger;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="sensorsPool"></param>
    /// <param name="sensorsPoolConfig"></param>
    /// <param name="provider"></param>
    /// <param name="logger"></param>
    public SensorsService(ISensorsPool sensorsPool, IOptions<SensorsPoolConfig> sensorsPoolConfig, IServiceProvider provider, ILogger<SensorsService> logger)
    {
        _sensorsPool = sensorsPool;
        _sensorsPoolConfig = sensorsPoolConfig;
        _provider = provider;
        _logger = logger;
    }
    
    public override async Task GetSensorsData(IAsyncStreamReader<SensorDataRequest> requestStream,
        IServerStreamWriter<SensorDataResponse> responseStream, ServerCallContext context)
    {
        while (!context.CancellationToken.IsCancellationRequested)
        {
            await GetRequest(requestStream, context);
            await GetResponse(responseStream, context);
            await Task.Delay(_sensorsPoolConfig.Value.UpdateTime, context.CancellationToken);
        }
    }

    private async Task GetResponse(IAsyncStreamWriter<SensorDataResponse> responseStream, ServerCallContext context)
    {
        var sensorsData = await _sensorsPool.GetSensorsData();

        foreach (var data in sensorsData)
        {
            if (context.CancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Response is cancelled!");
                break;
            }

            var result = new SensorDataResponse()
            {
                Guid = data.SensorGuid.ToString(),
                Humidity = data.Humidity,
                Temperature = data.Temperature,
                Ppm = data.Ppm,
                Timestamp = Timestamp.FromDateTime(data.TimeStamp)
            };
            
            await responseStream.WriteAsync(result, context.CancellationToken);
            
            _logger.LogInformation("Response is done!");
        }
        
    }

    private async Task GetRequest(IAsyncStreamReader<SensorDataRequest> requestStream, ServerCallContext context)
    {
        var sensorDataRequests = requestStream.ReadAllAsync();

        await foreach (var sensorDataRequest in sensorDataRequests)
        {
            if (context.CancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Request is cancelled!");
                break;
            }
            
            var delGuids = sensorDataRequest.Unsubscription.Select(g => new Guid(g)).ToArray();

            foreach (var guid in delGuids)
                await _sensorsPool.DeleteSensorsByGuid(guid);
            

        }
    }
}