using Ozon256.WeatherSensors.Contracts;
using Ozon256.WeatherSensors.SensorsEmulatorService.Models;
using Ozon256.WeatherSensors.SensorsEmulatorService.Options;
using Ozon256.WeatherSensors.SensorsEmulatorService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddMvcCore();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ISensorsPool, SensorsPool>();
builder.Services.Configure<SensorsPoolConfig>(builder.Configuration.GetSection(SensorsPoolConfig.SensorsPool));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();
app.UseEndpoints(
    b =>
    {
        b.MapControllers();
        app.MapGrpcService<SensorsService>();
    });

app.UseSwagger();
app.UseSwaggerUI();

app.Run();