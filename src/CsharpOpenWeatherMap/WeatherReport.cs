using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CsharpOpenWeatherMap
{
    public class WeatherReport
    {
        [JsonProperty("coord")]
        public OWMCoordinate Coordinates { get; set; } = new();

        [JsonProperty("weather")]
        public List<OWMWeather> Weather { get; set; } = new();

        [JsonProperty("base")]
        public string? Base { get; set; }

        [JsonProperty("main")]
        public OWMMain Main { get; set; } = new();

        [JsonProperty("visibility")]
        public int Visibility { get; set; }

        [JsonProperty("wind")]
        public OWMWind Wind { get; set; } = new();

        [JsonProperty("clouds")]
        public OWMClouds Clouds { get; set; } = new();

        [JsonProperty("dt")]
        public long DateTimeUnixUTC { get; set; }

        [JsonIgnore]
        public DateTime LastUpdated { get { return new DateTime(1970, 1, 1).AddSeconds(DateTimeUnixUTC).ToLocalTime(); } }

        [JsonProperty("sys")]
        public OWMSystem System { get; set; } = new();

        [JsonProperty("timezone")]
        public int Timezone { get; set; }

        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("cod")]
        public int Code { get; set; }

        // Custom Variables
        public bool IsDaytime { get { return DateTime.Compare(DateTime.Now, System.Sunset) < 0 && DateTime.Compare(DateTime.Now, System.Sunrise) > 0; } }
        public bool IsLight { get { return IsDaytime && Clouds?.Cloudiness < 95; } }
    }

    public class OWMCoordinate
    {
        [JsonProperty("lon")]
        public float Longitude { get; set; }

        [JsonProperty("lat")]
        public float Latitude { get; set; }
    }

    public class OWMWeather
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("main")]
        public string? Main { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("icon")]
        public string? Icon { get; set; }
    }

    public class OWMMain
    {
        [JsonProperty("temp")]
        public float Temperature { get; set; }

        [JsonProperty("feels_like")]
        public float FeelsLike { get; set; }

        [JsonProperty("temp_min")]
        public float TempMin { get; set; }

        [JsonProperty("temp_max")]
        public float TempMax { get; set; }

        [JsonProperty("pressure")]
        public int Pressure { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }
    }

    public class OWMWind
    {
        [JsonProperty("speed")]
        public float Speed { get; set; }

        [JsonProperty("deg")]
        public int Degree { get; set; }

        [JsonProperty("gust")]
        public float Gust { get; set; }
    }

    public class OWMClouds
    {
        [JsonProperty("all")]
        public int Cloudiness { get; set; }
    }

    [JsonConverter(typeof(UnixDateTimeConverter))]
    public class OWMSystem
    {
        [JsonProperty("country")]
        public string? CountryCode { get; set; }

        [JsonProperty("sunrise")]
        public DateTime Sunrise { get; set; }

        [JsonProperty("sunset")]
        public DateTime Sunset { get; set; }
    }

    public class UnixDateTimeConverter : JsonConverter<OWMSystem>
    {
        public override bool CanWrite => false;

        public override OWMSystem? ReadJson(JsonReader reader, Type objectType, OWMSystem? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            OWMSystem system = new();

            JObject jObject = JObject.Load(reader);

            system.CountryCode = jObject.Value<string>("country");
            system.Sunrise = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(jObject.Value<int>("sunrise")).ToLocalTime();
            system.Sunset = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(jObject.Value<int>("sunset")).ToLocalTime();

            return system;
        }

        public override void WriteJson(JsonWriter writer, OWMSystem? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class WeatherReportResponse
    {
        public bool RequestSuccessful { get; set; }
        public string? RequestError { get; set; }
        public WeatherReport? WeatherReport { get; set; }
    }
}