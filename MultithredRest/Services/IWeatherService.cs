namespace MultithredRest.Services
{
    using MultithredRest.Models.WeatherApi;

    public interface IWeatherService
    {
        void Dispose();

        Task<CityWeather?> GetCityWeather(string postCode, string countryCode, CancellationToken cancellationToken = default);
    }
}