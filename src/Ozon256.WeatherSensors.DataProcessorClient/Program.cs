
using Ozon256.WeatherSensors.Contracts;
using Ozon256.WeatherSensors.DataProcessorClient.SensorsDataStorage;
using Ozon256.WeatherSensors.DataProcessorClient.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcClient<Ozon256.WeatherSensors.SensorsEmulatorService.Sensors.SensorsClient>(
    options =>
    {
        options.Address = new Uri("https://localhost:7134/");
    });
builder.Services.AddSingleton<ISensorsDataStorage, SensorsDataStorage>();
builder.Services.AddHostedService<SensorsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();
app.UseEndpoints(
    b =>
    {
        //b.MapControllers();
        app.MapGrpcService<SensorsService>();
    });

app.Run();