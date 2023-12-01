using System.Text.Json.Serialization;

namespace isun.Models.ResponseModels
{
    public class ProblemDetails
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("status")]
        public int Status { get; set; }
        [JsonPropertyName("detail")]
        public string Detail { get; set; }
        [JsonPropertyName("instance")]
        public string Instance { get; set; }

        public override string ToString()
        {
            return $"Type: {this.Type}, Title: {this.Title}, Status: {this.Status}, Detail: {this.Detail}, Instance: {this.Instance}";
        }
    }
}