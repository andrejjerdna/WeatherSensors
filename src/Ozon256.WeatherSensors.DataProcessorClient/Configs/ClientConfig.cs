using Grpc.Core;
using Grpc.Net.Client.Configuration;

namespace Ozon256.WeatherSensors.DataProcessorClient.Configs;

public class ClientConfig
{
    public static ServiceConfig GetServiceConfig()
    {
        var methodConfig = new MethodConfig
        {
            Names = { MethodName.Default },
            RetryPolicy = new RetryPolicy
            {
                MaxAttempts = 5,
                InitialBackoff = TimeSpan.FromSeconds(1),
                MaxBackoff = TimeSpan.FromSeconds(5),
                BackoffMultiplier = 1.5,
                RetryableStatusCodes = { StatusCode.Unavailable },
            }
        };
        
        return new ServiceConfig()
        {
            MethodConfigs = { methodConfig }
        };
    }
}