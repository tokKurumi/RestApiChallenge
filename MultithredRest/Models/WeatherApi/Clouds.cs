namespace MultithredRest.Models.WeatherApi
{
    using System.Text.Json.Serialization;

    public class Clouds
    {
        [JsonPropertyName("all")]
        public int All { get; set; }
    }
}
