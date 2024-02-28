using Example.Data;
using Example.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultithredRest.HostDefaults;

var host = MultithreadRestHost
    .CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        services.AddDbContext<CityDbContext>(options =>
        {
            options.UseInMemoryDatabase("TestDB");
        });

        services.AddSingleton<IWeatherService, WeatherService>();
    })
    .Build();

await host.RunAsync();