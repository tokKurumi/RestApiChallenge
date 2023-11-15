namespace MultithredRest.Models.WeatherApi
{
    using System.Text.Json.Serialization;

    public class Coord
    {
        [JsonPropertyName("lon")]
        public double Lon { get; set; }

        [JsonPropertyName("lat")]
        public double Lat { get; set; }
    }
}
