using System.Text.Json.Serialization;

namespace isun.Responses
{
    public class WeatherResponse
    {
        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("temperature")]
        public int Temperature { get; set; }

        [JsonPropertyName("precipitation")]
        public int Precipitation { get; set; }

        [JsonPropertyName("windSpeed")]
        public double WindSpeed { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        public override string ToString()
        {
            return $"City {City}, Temperature {Temperature}, Precipitation {Precipitation}, WindSpeed {WindSpeed}, Summary {Summary}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            var wr = (WeatherResponse)obj;
            return this.City == wr.City &&
                this.Temperature == wr.Temperature &&
                this.Precipitation == wr.Precipitation &&
                this.WindSpeed == wr.WindSpeed &&
                this.Summary == wr.Summary;
        }
    }
}
