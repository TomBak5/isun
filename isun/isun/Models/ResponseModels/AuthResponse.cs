using System.Text.Json.Serialization;

namespace isun.Responses
{
    public class AuthResponse
    {
        [JsonPropertyName("token")]
        public string TokenValue { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            return this.TokenValue == ((AuthResponse)obj).TokenValue;
        }
    }
}
