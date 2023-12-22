using Example.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultithredRest.HostDefaults;

var host = MultithreadRestHost
    .CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        services.AddSingleton<IWeatherService, WeatherService>();
    })
    .Build();

host.RunConfigured();