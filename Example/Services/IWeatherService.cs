namespace Example.Services;

using Example.Models.WeatherApi;

public interface IWeatherService
{
    Task<CityWeather?> GetCityWeather(string postCode, string countryCode, CancellationToken cancellationToken = default);
}