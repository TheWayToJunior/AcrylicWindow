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
using System.Threading.Tasks;

namespace AcrylicWindow.Providers
{
    public class AuthorizationProvider : IAuthorizationProvider
    {
        private readonly IAuthorizationService<JwtResponse> _authorizationService;
        private readonly ISessionService<UserSession> _sessionService;
        private readonly HttpClient _httpClient;

        private AuthenticationState Anonymous => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        public AuthorizationProvider(IAuthorizationService<JwtResponse> authorizationService, ISessionService<UserSession> sessionService,
            HttpClient httpClient)
        {
            _authorizationService = Has.NotNull(authorizationService);
            _sessionService = Has.NotNull(sessionService);
            _httpClient = Has.NotNull(httpClient);
        }

        private string sessionPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "session.json");

        public AuthenticationState GetAuthenticationState()
        {
            if (!_sessionService.TryRecover(sessionPath, out UserSession session))
            {
                return Anonymous;
            }

            return BuildAuthenticationState(session.Token);
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
            await _sessionService.SaveAsync(sessionPath, new UserSession { Token = token });
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

