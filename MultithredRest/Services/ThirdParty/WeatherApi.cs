namespace MultithredRest.Services.ThirdParty
{
    using MultithredRest.Models.Weather;

    public class WeatherApi : IWeatherApi
    {
        public Task<WeatherCity> GetCityWeather(string city)
        {
            return city switch
            {
                "Саранск" => Task.FromResult(new WeatherCity(city, "Хорошая погода")),
                "Москва" => Task.FromResult(new WeatherCity(city, "Облачно")),
                "Казань" => Task.FromResult(new WeatherCity(city, "Идёт дождь")),
                _ => Task.FromResult(new WeatherCity(city, "Хорошая погода"))
            };
        }
    }
}