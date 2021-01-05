using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.Model;
using IdentityModel.Client;
using System;
using System.IO;
using System.Net.Http;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.Providers
{
    public class AuthorizationProvider : IAuthorizationProvider
    {
        private readonly IAuthorizationService<JwtResponse> _authorizationService;
        private readonly ISessionService<UserSession> _sessionService;
        private readonly HttpClient _httpClient;

        private readonly string _sessionPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "session.json");

        private string RefreshToken { get; set; }
        private AuthenticationState Anonymous => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        private AuthenticationState _authenticationState;
        public AuthenticationState AuthenticationState => _authenticationState ?? GetAuthenticationState();

        public AuthorizationProvider(IAuthorizationService<JwtResponse> authorizationService, ISessionService<UserSession> sessionService,
            HttpClient httpClient)
        {
            _authorizationService = Has.NotNull(authorizationService);
            _sessionService = Has.NotNull(sessionService);
            _httpClient = Has.NotNull(httpClient);
        }

        public async Task<AuthenticationState> Login(string email, SecureString password)
        {
            AuthorizationResult<JwtResponse> result = await _authorizationService.AuthorizeAsync(email, password);

            return await SignInAsync(result);
        }

        public async Task<AuthenticationState> ExtendSession()
        {
            AuthorizationResult<JwtResponse> response = await _authorizationService.RefreshAsync(RefreshToken);

            return await SignInAsync(response);
        }

        public async Task Logout()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            await AuthenticationStateChangedAsync(accessToken: string.Empty, refreshToken: string.Empty);
        }

        private AuthenticationState GetAuthenticationState()
        {
            if (!_sessionService.TryRecover(_sessionPath, out UserSession session))
            {
                return Anonymous;
            }

            RefreshToken = session.RefreshToken;
            return BuildAuthenticationState(session.AccessToken);
        }

        private async Task<AuthenticationState> SignInAsync(AuthorizationResult<JwtResponse> authorization) 
        {
            if (!authorization.IsSuccess)
            {
                AuthenticationState anonymous = Anonymous;
                anonymous.ErrorMessage = authorization.ErrorMessage;

                return anonymous;
            }

            string token = authorization.Response.AccessToken;

            return await AuthenticationStateChangedAsync(token, authorization.Response.RefreshToken);
        }

        private async Task<AuthenticationState> AuthenticationStateChangedAsync(string accessToken, string refreshToken)
        {
            RefreshToken = refreshToken;

            await _sessionService.SaveAsync(_sessionPath, new UserSession 
            {
                AccessToken = accessToken, 
                RefreshToken = refreshToken 
            });

            return BuildAuthenticationState(accessToken);
        }

        private AuthenticationState BuildAuthenticationState(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return _authenticationState = Anonymous;
            }

            _httpClient.SetBearerToken(token);

            return _authenticationState = new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity(token.ParseClaimsFromJwt(), "jwt")));
        }
    }
}

