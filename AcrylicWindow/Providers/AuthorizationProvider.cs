using AcrylicWindow.Helpers;
using AcrylicWindow.IContract;
using AcrylicWindow.IContract.IProviders;
using AcrylicWindow.Model;
using IdentityModel.Client;
using System;
using System.IO;
using System.Net.Http;
using System.Security;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcrylicWindow.Providers
{
    public class AuthorizationProvider : IAuthorizationProvider
    {
        private readonly IAuthorizationService<JwtResponse> _authorizationService;
        private readonly HttpClient _httpClient;

        private AuthenticationState Anonymous => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        public AuthorizationProvider(IAuthorizationService<JwtResponse> authorizationService, HttpClient httpClient)
        {
            _authorizationService = Has.NotNull(authorizationService);
            _httpClient = Has.NotNull(httpClient);
        }

        private string sessionPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "session.json");

        public AuthenticationState GetAuthenticationStateAsync()
        {
            if (!File.Exists(sessionPath))
            {
                return Anonymous;
            }

            string token = JsonSerializer.Deserialize<UserSession>(File.ReadAllText(sessionPath)).Token;

            return BuildAuthenticationState(token);
        }

        public async Task<AuthenticationState> Login(string email, SecureString password)
        {
            AuthorizationResult<JwtResponse> result = await _authorizationService.AuthorizeAsync(email, password);

            if (!result.IsSuccess)
            {
                AuthenticationState anonymous = Anonymous;
                anonymous.ErrorMessage = result.ErrorMessage;

                return anonymous;
            }

            string token = result.Response.AccessToken;
            await AuthenticationStateChanged(token);

            return BuildAuthenticationState(token);
        }

        public async Task Logout()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            await AuthenticationStateChanged(token: string.Empty);
        }

        private async Task AuthenticationStateChanged(string token)
        {
            using (FileStream fs = new FileStream(sessionPath, FileMode.Create))
            {
                await JsonSerializer.SerializeAsync(fs, new UserSession { Token = token });
            }
        }

        private AuthenticationState BuildAuthenticationState(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Anonymous;
            }

            _httpClient.SetBearerToken(token);

            return new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity(token.ParseClaimsFromJwt(), "jwt")));
        }
    }
}

