namespace Example.Services
{
    using Example.Models.WeatherApi;

    public interface IWeatherService
    {
        void Dispose();

        Task<CityWeather?> GetCityWeather(string postCode, string countryCode, CancellationToken cancellationToken = default);
    }
}