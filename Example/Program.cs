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
            options.UseNpgsql("Host=localhost;Port=5432;Database=MathZ;Username=kurumi;Password=root");
        });

        services.AddSingleton<IWeatherService, WeatherService>();
    })
    .Build();

host.RunConfigured();