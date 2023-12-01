using System.Text.Json.Serialization;

namespace isun.Models.ResponseModels
{
    public class AuthResponse
    {
        [JsonPropertyName("token")]
        public string TokenValue { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            return TokenValue == ((AuthResponse)obj).TokenValue;
        }
    }
}
