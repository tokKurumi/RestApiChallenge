namespace MultithredRest.Services.ThirdParty
{
    using MultithredRest.Models.Weather;

    public interface IWeatherApi
    {
        Task<WeatherCity> GetCityWeather(string city);
    }
}