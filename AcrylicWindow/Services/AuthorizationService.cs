using AcrylicWindow.IContract;
using AcrylicWindow.Model;
using IdentityModel.Client;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcrylicWindow.Services
{
    public class AuthorizationService<TResponse> : IAuthorizationService<TResponse>
        where TResponse : class, new()
    {
        public async Task<AuthorizationResult<TResponse>> AuthorizeAsync(UserInfo userInfo)
        {
            return await AuthorizeAsync(userInfo.Email, userInfo.Password);
        }

        /// <summary>
        /// Performs authorization on a remote server using the Bearer authentication scheme based on OAuth 2.0
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        public async Task<AuthorizationResult<TResponse>> AuthorizeAsync(string email, SecureString password)
        {
            HttpClient httpClient = new HttpClient();

            var discoveryDocument = await httpClient.GetDiscoveryDocumentAsync(ConfigurationManager.AppSettings["Address"]);
            var credential = new NetworkCredential(email, password);

            var response = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest()
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = ConfigurationManager.AppSettings["ClientId"],
                ClientSecret = ConfigurationManager.AppSettings["ClientSecret"],
                Scope = ConfigurationManager.AppSettings["Scope"],

                /// Used Grant Types ResourceOwnerPassword
                UserName = credential.UserName,
                Password = credential.Password
            });

            httpClient.Dispose();

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
