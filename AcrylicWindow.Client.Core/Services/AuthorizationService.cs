using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.Model;
using IdentityModel.Client;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.Services
{
    public class AuthorizationService<TResponse> : IAuthorizationService<TResponse>
        where TResponse : class, new()
    {
        private readonly HttpClient _httpClient;

        public AuthorizationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Performs authorization on a remote server using the Bearer authentication scheme based on OAuth 2.0
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        public async Task<AuthorizationResult<TResponse>> AuthorizeAsync(string email, SecureString password)
        {
            var discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync(ConfigurationManager.AppSettings["Address"]);
            var credential = new NetworkCredential(email, password);

            var response = await _httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest()
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = ConfigurationManager.AppSettings["ClientId"],
                ClientSecret = ConfigurationManager.AppSettings["ClientSecret"],
                Scope = ConfigurationManager.AppSettings["Scope"],

                /// Used Grant Types ResourceOwnerPassword
                UserName = credential.UserName,
                Password = credential.Password
            });

            var result = new AuthorizationResult<TResponse>();

            if (response.IsError)
            {
                result.ErrorMessage = string.IsNullOrEmpty(response.ErrorDescription) ?
                    "No response from the server" :
                    response.ErrorDescription;

                return result;
            }

            var responseString = await response.HttpResponse.Content.ReadAsStringAsync();

            var deserializeResult = JsonSerializer.Deserialize<TResponse>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            result.Response = deserializeResult;

            return result;
        }
    }
}
