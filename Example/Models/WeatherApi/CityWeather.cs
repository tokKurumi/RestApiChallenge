namespace Example.Models.WeatherApi
{
    using System.Text.Json.Serialization;

    public class CityWeather
    {
        [JsonPropertyName("coord")]
        public Coord Coord { get; set; } = new Coord();

        [JsonPropertyName("weather")]
        public List<Weather> Weather { get; set; } = new List<Weather>();

        [JsonPropertyName("base")]
        public string Base { get; set; } = string.Empty;

        [JsonPropertyName("main")]
        public Main Main { get; set; } = new Main();

        [JsonPropertyName("visibility")]
        public int Visibility { get; set; }

        [JsonPropertyName("wind")]
        public Wind Wind { get; set; } = new Wind();

        [JsonPropertyName("clouds")]
        public Clouds Clouds { get; set; } = new Clouds();

        [JsonPropertyName("dt")]
        public int Dt { get; set; }

        [JsonPropertyName("sys")]
        public Sys Sys { get; set; } = new Sys();

        [JsonPropertyName("timezone")]
        public int Timezone { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("cod")]
        public int Cod { get; set; }
    }
}