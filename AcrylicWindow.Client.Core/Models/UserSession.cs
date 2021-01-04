using System.Text.Json.Serialization;

namespace AcrylicWindow.Client.Core.Model
{
    public class UserSession
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
