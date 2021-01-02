using System.Text.Json.Serialization;

namespace AcrylicWindow.Client.Core.Model
{
    public class JwtResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string IssuedTokenType { get; set; }
    }
}
