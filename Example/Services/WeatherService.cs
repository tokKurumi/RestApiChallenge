namespace Example.Services
{
    using System.Text.Json;
    using Example.Models.WeatherApi;

    public class WeatherService : IDisposable, IWeatherService
    {
        private readonly HttpClient _api = new HttpClient() { BaseAddress = new Uri(@"https://api.openweathermap.org/") };
        private bool _disposing = false;

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposing)
            {
                if (disposing)
                {
                    _api.Dispose();
                }
            }

            _disposing = true;
        }
    }
}
