namespace Example.Services;

using System.Text.Json;
using Example.Models.WeatherApi;

public class WeatherService(HttpClient api)
    : IWeatherService
{
    private readonly HttpClient _api = api;

    public async Task<CityWeather?> GetCityWeather(string postCode, string countryCode, CancellationToken cancellationToken = default)
    {
        var cityWeatherResponse = await _api.GetAsync(@$"data/2.5/weather?q={postCode},{countryCode}&APPID={Resources.Weather.WeatherApi.key}", cancellationToken);

        if (!cityWeatherResponse.IsSuccessStatusCode)
        {
            throw new HttpRequestException("Invalid request");
        }

        return await JsonSerializer.DeserializeAsync<CityWeather>(
            await cityWeatherResponse.Content.ReadAsStreamAsync(cancellationToken),
            cancellationToken: cancellationToken);
    }
}
