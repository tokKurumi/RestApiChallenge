namespace MultithredRest.Endpoints
{
    using System.Net;
    using System.Text;
    using MultithredRest.Core;
    using MultithredRest.Models.Weather;
    using MultithredRest.Services.ThirdParty;
    using Newtonsoft.Json;

    public class Weather : EndpointBase
    {
        public Weather(HttpMethod method)
            : base(method)
        {
        }

        protected override ReadOnlySpan<byte> GenerateResponseBuffer(HttpListenerRequest request)
        {
            var data = DeserializeBody<WeatherParam>(request);

            var weatherApi = new WeatherApi();
            var weather = weatherApi.GetCityWeather(data.City);

            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { Message = weather }, Formatting.Indented));
        }
    }
}
